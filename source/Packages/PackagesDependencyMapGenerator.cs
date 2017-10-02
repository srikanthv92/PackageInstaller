using PackageInstallerExercise.Interfaces;
using System;

namespace PackageInstallerExercise.Packages {

  /// <summary>
  /// Package Dependency Map Generator
  /// </summary>
  public class PackagesDependencyMapGenerator : IDependencyMapGenerator {

    private char _delimiter;
    private IDependencyMap _packageDependencyMap;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="delimiter">Delimiter separating the Package from the dependency</param>
    public PackagesDependencyMapGenerator(char delimiter, IDependencyMap packageDependencyMap) {
      _delimiter = delimiter;
      _packageDependencyMap = packageDependencyMap;
    }

    /// <summary>
    /// Create Dependency Map
    /// </summary>
    /// <param name="definitions"></param>
    /// <returns>Array of dependencies in their build order</returns>
    public string[] CreateMap(string[] definitions) {
      FillMap(definitions);
      return this._packageDependencyMap.GetMap();
    }

    /// <summary>
    /// Fills dependency map with the definitions provided
    /// </summary>
    /// <param name="definitions"></param>
    private void FillMap(string[] definitions) {

      foreach (string definition in definitions) {

        // Strip out package:dependency from string
        string[] packageAndDependency = definition.Split(this._delimiter);

        if (packageAndDependency.Length != 2) {
          throw new FormatException("Dependency string is not in the correct format.");
        }

        string packageName = packageAndDependency[0].Trim();
        string dependencyName = packageAndDependency[1].Trim();

        this._packageDependencyMap.AddPackage(packageName, dependencyName);

      }

    }

  }
}
