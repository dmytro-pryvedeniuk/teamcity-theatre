using System.Collections.Generic;
using System.Linq;

namespace TeamCityTheatre.Core.ApplicationModels
{
  public class Configuration
  {
    public Configuration()
    {
      Views = Enumerable.Empty<View>();
      DisplayBranches = Enumerable.Empty<string>();
      UserSettings = new UserSettings();
    }

    public IEnumerable<View> Views { get; set; }

    public UserSettings UserSettings { get; set; }

    public IEnumerable<string> DisplayBranches { get; set; }
  }
}