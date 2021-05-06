using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace PW.LaunchPad
{
  /// <summary>
  /// Management of application registrations for LaunchPad.
  /// </summary>
  public static class RegistrationManager
  {
    private const string RegPath = @"Software\PW\AppRegistration";

    /// <summary>
    /// Registers an application with LaunchPad.
    /// </summary>
    public static void Register(string title, string path)
    {
      if (string.IsNullOrWhiteSpace(title)) throw new System.ArgumentException($"'{nameof(title)}' cannot be null or whitespace", nameof(title));
      if (string.IsNullOrWhiteSpace(path)) throw new System.ArgumentException($"'{nameof(path)}' cannot be null or whitespace", nameof(path));

      using var key = GetAppRegKey();
      key.SetValue(title, path);
    }

    /// <summary>
    /// Unregisters an application with LaunchPad.
    /// </summary>
    public static void UnRegister(string title)
    {
      if (title is null) throw new System.ArgumentNullException(nameof(title));

      using var key = GetAppRegKey();
      key.DeleteValue(title, false);
    }

    /// <summary>
    /// Returns a list of all existing application registrations.
    /// </summary>
    public static List<(string Title, string Path)> GetRegistrations()
    {
      using var key = GetAppRegKey();
      return key.GetValueNames()
              .OrderBy(appTitle => appTitle)
              .Select(appTitle => (appTitle, key.GetValue(appTitle)!.ToString()!))
              .ToList();
    }

    /// <summary>
    /// Opens the registry key used by LaunchPad.
    /// </summary>   
    private static RegistryKey GetAppRegKey() => Registry.CurrentUser.CreateSubKey(RegPath);




  }
}
