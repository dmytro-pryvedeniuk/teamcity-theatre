using System;
using System.Collections.Generic;

namespace TeamCityTheatre.Core.ApplicationModels
{
  public class View
  {
    public View()
    {
      Tiles = new List<Tile>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public int DefaultNumberOfBranchesPerTile { get; set; }
    public int NumberOfColumns { get; set; }
    public ICollection<Tile> Tiles { get; set; }
  }

  public class UserSettings
  {
    public UserSettings()
    {
      Users = new List<User>();
    }
    public ICollection<User> Users { get; set; }
    public string NullImageUrl { get; set; }
  }

  public class User
  {
    public string Name { get; set; }
    public string ImageUrl { get; set; }
  }
}