using System;

namespace TeamCityTheatre.Core.Models {
  public class BuildChange : IDetailedBuildChange {
    public string Id { get; set; }
    public string Version { get; set; }
    public string Username { get; set; }
    public DateTime Date { get; set; }
    public string Href { get; set; }
    public string WebLink { get; set; }
    public string UserImageUrl { get; set; }

    public override string ToString() {
      return string.Format("Id: {0}, Version: {1}, Username: {2}, Date: {3}", Id, Version, Username, Date);
    }
  }
}