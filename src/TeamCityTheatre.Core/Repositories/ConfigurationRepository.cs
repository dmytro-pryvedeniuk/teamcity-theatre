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
    readonly string _workspace;
    readonly string _configurationFile;
    private DateTime _lastWriteTime;
    private Configuration _lastConfiguration;

    public ConfigurationRepository(IOptionsSnapshot<StorageOptions> storageOptionsSnapshot) {
      var storageOptions = storageOptionsSnapshot.Value;
      _configurationFile = storageOptions.ConfigurationFile;
      _workspace = Path.GetDirectoryName(_configurationFile);
    }

    public Configuration GetConfiguration() {
      EnsureConfigurationFileExists();

      var currentWriteTime = File.GetLastWriteTime(_configurationFile);
      if (currentWriteTime == _lastWriteTime)
      {
        return _lastConfiguration;        
      }
      _lastConfiguration = JsonConvert.DeserializeObject<Configuration>(
        File.ReadAllText(_configurationFile)) ?? new Configuration();
      _lastWriteTime = currentWriteTime;
      return _lastConfiguration;
    }

    public void SaveConfiguration(Configuration configuration) {
      EnsureConfigurationFileExists();
      File.WriteAllText(_configurationFile, JsonConvert.SerializeObject(configuration, Formatting.Indented));
    }

    void EnsureConfigurationFileExists() {
      if (!Directory.Exists(_workspace))
        Directory.CreateDirectory(_workspace);
      if (!File.Exists(_configurationFile))
        File.Create(_configurationFile).Dispose();
    }
  }
}