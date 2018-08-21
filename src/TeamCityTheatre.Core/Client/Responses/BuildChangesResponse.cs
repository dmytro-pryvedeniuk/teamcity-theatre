using System.Collections.Generic;

namespace TeamCityTheatre.Core.Client.Responses {
  public class BuildChangesResponse {
    public int Count { get; set; }
    public List<BuildChangeResponse> Change { get; set; }
  }
}