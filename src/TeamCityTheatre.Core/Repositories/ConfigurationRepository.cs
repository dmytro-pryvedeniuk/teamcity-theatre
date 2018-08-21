using System;
using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TeamCityTheatre.Core.ApplicationModels;
using TeamCityTheatre.Core.Options;

namespace TeamCityTheatre.Core.Repositories {
  public interface IConfigurationRepository {
    Configuration GetConfiguration();
    void SaveConfiguration(Configuration configuration);
  }

  public class ConfigurationRepository : IConfigurationRepository {
    readonly DirectoryInfo _workspace;
    readonly FileInfo _configurationFile;
    private DateTime _lastWriteTime;
    private Configuration _lastConfiguration;

    public ConfigurationRepository(IOptionsSnapshot<StorageOptions> storageOptionsSnapshot) {
      var storageOptions = storageOptionsSnapshot.Value;
      _configurationFile = new FileInfo(storageOptions.ConfigurationFile);
      _workspace = _configurationFile.Directory;
    }

    public Configuration GetConfiguration() {
      EnsureConfigurationFileExists();

      var currentWriteTime = File.GetLastWriteTime(_configurationFile.FullName);
      if (currentWriteTime == _lastWriteTime)
      {
        return _lastConfiguration;        
      }
      _lastConfiguration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(_configurationFile.FullName)) ?? new Configuration();
      _lastWriteTime = currentWriteTime;
      return _lastConfiguration;
    }

    public void SaveConfiguration(Configuration configuration) {
      EnsureConfigurationFileExists();
      File.WriteAllText(_configurationFile.FullName, JsonConvert.SerializeObject(configuration, Formatting.Indented));
    }

    void EnsureConfigurationFileExists() {
      if (!_workspace.Exists)
        _workspace.Create();
      if (!_configurationFile.Exists)
        _configurationFile.Create().Dispose();
    }
  }
}