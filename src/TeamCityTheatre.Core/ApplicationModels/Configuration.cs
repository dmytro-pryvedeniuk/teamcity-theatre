using System.Collections.Generic;
using System.Linq;

namespace TeamCityTheatre.Core.ApplicationModels
{
  public class Configuration
  {
    public Configuration()
    {
      Views = Enumerable.Empty<View>();
      UserSettings = new UserSettings();
    }

    public IEnumerable<View> Views { get; set; }

    public UserSettings UserSettings { get; set; }
  }
}