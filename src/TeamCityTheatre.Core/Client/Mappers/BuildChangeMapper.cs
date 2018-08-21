using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;
using TeamCityTheatre.Core.Repositories;

namespace TeamCityTheatre.Core.Client.Mappers
{
  public interface IBuildChangeMapper
  {
    BuildChange Map(BuildChangeResponse buildChange);
    IReadOnlyCollection<BuildChange> Map(BuildChangesResponse buildChanges);
  }

  public class BuildChangeMapper : IBuildChangeMapper
  {
    private IConfigurationRepository _configurationRepository;

    public BuildChangeMapper(IConfigurationRepository configurationRepository)
    {
      _configurationRepository = configurationRepository;
    }

    public BuildChange Map(BuildChangeResponse buildChange)
    {
      if (buildChange == null)
        return null;
      return new BuildChange
      {
        Date = buildChange.Date,
        Href = buildChange.Href,
        Id = buildChange.Id,
        Username = buildChange.Username,
        Version = buildChange.Version,
        WebLink = buildChange.WebLink,
        UserImageUrl = GetUserImageUrl(buildChange.Username)
      };
    }

    private string GetUserImageUrl(string username)
    {
      var imageUrl = _configurationRepository.GetConfiguration()
        .UserSettings.Users
        .Where(u => string.Equals(u.Name, username, StringComparison.OrdinalIgnoreCase))
        .Select(u => u.ImageUrl)
        .FirstOrDefault();

      if (imageUrl == null)
      {
        return _configurationRepository.GetConfiguration().UserSettings.NullImageUrl;
      }

      return imageUrl;
    }

    public IReadOnlyCollection<BuildChange> Map(BuildChangesResponse buildChanges)
    {
      if (buildChanges == null || buildChanges.Change == null)
        return new List<BuildChange>();
      return buildChanges.Change.Select(Map).ToList();
    }
  }
}