using System;

namespace TeamCityTheatre.Core.Models {
  /*
   * <change id="51798" version="4c493b9fcc2af97662521fc064153a9dbb98e217" username="wnaessens" 
                date="20150227T104740+0100" href="/httpAuth/app/rest/changes/id:51798" 
                webLink="http://vm64-teamcity:8001/viewModification.html?modId=51798&personal=false" />
   */

  public interface IDetailedBuildChange {
    string Id { get; set; }
    string Version { get; set; }
    string Username { get; set; }
    DateTime Date { get; set; }
    string Href { get; set; }
    string WebLink { get; set; }
    string UserImageUrl { get; set; }
  }
}