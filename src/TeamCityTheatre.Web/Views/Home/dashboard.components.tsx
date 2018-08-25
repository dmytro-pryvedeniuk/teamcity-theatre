﻿import { createElement, MouseEvent } from "react";
import { IView, IViewData, ITileData, BuildStatus, IDetailedBuild } from "../Shared/contracts";
import * as parse from "date-fns/parse";
import * as addSeconds from "date-fns/add_seconds";
import * as distanceInWordsToNow from "date-fns/distance_in_words_to_now";

/**
 * Root dispatching component
 */
export const Dashboard =
  (props: { views: IView[] | null, selectedView: IView | null, selectedViewData: IViewData | null }) => {
    if (props.views === null)
      return <div>
        <i className="fa fa-spin fa-cog" /> Loading views
             </div>;

    if (props.selectedView === null)
      return <Views views={props.views} />;

    if (props.selectedViewData === null)
      return <div>
        <i className="fa fa-spin fa-cog" /> Loading view data
             </div>;

    return <View view={props.selectedView} data={props.selectedViewData} />;
  };

/**
 * List of views to choose from
 */
const Views = (props: { views: IView[] }) => (
  <div id="views">
    {props.views.map(view => (
      <a className="btn btn-primary view" id={view.id} key={view.id} href={`dashboard/${view.name.toLowerCase()}`}>
        {view.name} <span className="badge">{view.tiles.length} tiles</span>
      </a>))}
  </div>
);

const tryRequestFullScreen = (event: MouseEvent<HTMLButtonElement>) => {
  const button = event.currentTarget as HTMLButtonElement;
  const view = button.parentNode as HTMLDivElement;
  if (view.requestFullscreen) view.requestFullscreen();
  if (view.webkitRequestFullScreen) view.webkitRequestFullScreen();
  if (view.webkitRequestFullscreen) view.webkitRequestFullscreen();
};

/**
 * Details of a single view
 */
const View = (props: { view: IView, data: IViewData }) => (
  <div className="view" id={props.view.id}>
    <button role="button" className="btn btn-primary btn-xs" onClick={tryRequestFullScreen}>
      <i className="fa fa-expand" /> Full screen
    </button>
    <div id="tiles">
      <div className="tiles-wrapper">
        {props.data.tiles.map(tile => <Tile key={tile.id} view={props.view} data={tile} />)}
      </div>
    </div>
  </div>
);

/**
 * A single tile of a view
 */
const Tile = (props: { view: IView, data: ITileData }) => {
  const buildStatus = BuildStatus[props.data.combinedBuildStatus].toLowerCase();
  const height = `height-${props.view.defaultNumberOfBranchesPerTile}`;
  const numberOfColumns = props.view.numberOfColumns || 6;
  const width = `width-${numberOfColumns}`;
  return (
    <div id={props.data.id} className={`tile ${buildStatus} ${height} ${width}`}>
      <h4 className="tile-title">{props.data.label}</h4>
      <div className="tile-builds">
        {props.data.builds.map(build => <Build key={build.id} build={build} />)}
      </div>
    </div>
  );
};

/**
 * A single build in a tile
 */
const Build = (props: { build: IDetailedBuild }) => {
  const { build } = props;
  const isFinished = build.state === "finished";
  const isRunning = build.state === "running";
  const isSuccess = build.status === BuildStatus.Success;

  const buildStatus = BuildStatus[build.status].toLowerCase();
  const percentageCompleted = isFinished ? 100 : build.percentageComplete;
  const progressBarTheme = isSuccess ? "progress-bar-success" : "progress-bar-danger";
  const progressBarAnimation = isRunning ? "progress-bar-striped active" : "";

  return (
    <div id={build.id} className={`tile-build ${buildStatus}`}>
      <div className="progress">
        <a href={build.webUrl} target="_blank">
          <div className={`progress-bar ${progressBarTheme} ${progressBarAnimation}`} style={{ width: `${percentageCompleted}%` }}>
            {<UserImage build={build} />}
            <div className="buildCaption">
              {<Branch build={build} />}
              {isFinished ? <FinishDate build={build} /> : null}
              {isRunning ? <TimeRemaining build={build} /> : null}
              {<UserName build={build} />}
            </div>
          </div>
        </a>
      </div>
    </div>
  );
};

const Branch = (props: { build: IDetailedBuild }) => {
  const isDefaultBranch = props.build.isDefaultBranch;
  const displayBranchName = props.build.displayBranchName;
  return isDefaultBranch
    ? <span className="branch"><i className="fa fa-star" /> {displayBranchName}</span>
    : <span className="branch">{displayBranchName}</span>;
};

const UserName = (props: { build: IDetailedBuild }) => {
  if (props.build.lastChanges && props.build.lastChanges.length > 0) {
    const userName = props.build.lastChanges[0].username;
    if (userName)
      return <span className="remaining">{userName}</span>;
  }
  return (null);
}

const UserImage = (props: { build: IDetailedBuild }) => {
  if (props.build.lastChanges && props.build.lastChanges.length > 0) {
    const imageSource = props.build.lastChanges[0].userImageUrl;
    if (imageSource)
      return <img className="imgUserImage" src={imageSource} />;
  }
  return (null);
}

const FinishDate = (props: { build: IDetailedBuild }) => {
  const { build } = props;
  const isSuccess = build.status === BuildStatus.Success;
  const theme = isSuccess ? "success" : "danger";

  const finishDate = parse(build.finishDate);
  const differenceWithNow = distanceInWordsToNow(finishDate, { includeSeconds: true, addSuffix: true });
  return (
    <span className="execution-timestamp">
      <span className={`build-number label label-${theme}`}>{build.number}</span>
      {` finished ${differenceWithNow}`}
    </span>
  );
};

const TimeRemaining = (props: { build: IDetailedBuild }) => {
  const { build } = props;
  const isSuccess = build.status === BuildStatus.Success;
  const theme = isSuccess ? "success" : "danger";

  const estimatedFinishDate = addSeconds(parse(props.build.startDate), props.build.estimatedTotalSeconds);
  const differenceWithNow = distanceInWordsToNow(estimatedFinishDate, { includeSeconds: true, addSuffix: true });
  return (
    <span className="remaining">
      <span className={`build-number label label-${theme}`}>{build.number}</span>
      {` will finish ${differenceWithNow}`}
    </span>
  );
};