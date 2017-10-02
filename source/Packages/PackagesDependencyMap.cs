using PackageInstallerExercise.Interfaces;
using PackageInstallerExercise.Packages.Interfaces;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PackageInstallerExercise.Packages.Exceptions;

namespace PackageInstallerExercise.Packages {

  /// <summary>
  /// Container for holding the mapping between package dependencies
  /// </summary>
  public class PackagesDependencyMap<P> : IDependencyMap
    where P : IPackage, new() {

    public List<IPackage> Packages { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public PackagesDependencyMap() {
      this.Packages = new List<IPackage>();
    }

    /// <summary>
    /// Add Package to dependency map.
    /// </summary>
    /// <param name="packageName">Package Name</param>
    /// <param name="dependencyName">Dependency Name</param>
    /// <remarks>When dependency already exists in map, will link instead of create</remarks>
    public IPackage AddPackage(string packageName, string dependencyName = null) {

      // See if package already exists in the list
      var existingPackage = FindPackage(packageName);

      // If package already has a dependency, throw duplicate error
      if (existingPackage != null && existingPackage.Dependency != null) {
        throw new PackageDuplicateException(existingPackage);
      }

      var newPackages = new List<IPackage>();

      IPackage dependency = default(P);
      // When dependencyName is passed, find it or create it
      if (!string.IsNullOrWhiteSpace(dependencyName)) {
        dependency = CreateOrFindPackage(dependencyName, newPackages);
      }

      // Create new Package or get it from the package list adding its dependency
      var package = CreateOrFindPackage(packageName, newPackages);
      package.Dependency = dependency;

      // Determine if it contains a cycle
      if (PackageHasCycle(package, package.Name)) {
        throw new PackageContainsCycleException(package);
      }

      // We have gotten this far, so add the newly created pages to the package list
      this.Packages.AddRange(newPackages);
      return this.Packages.Last();

    }

    /// <summary>
    /// Create or Find the new Package and add to package list
    /// </summary>
    /// <param name="packageName">Name of the Package to create</param>
    /// <param name="packages">Temporary package list to add the package if created</param>
    /// <returns>Created of Found Package</returns>
    private IPackage CreateOrFindPackage(string packageName, IList packages = null) {

      // See if package already exists in the list
      var package = FindPackage(packageName);

      // If package doesn't already exist, create a new instance
      if (package == null) {

        package = new P() {
          Name = packageName
        };

        // If no packages passed, default to that of the class
        if (packages == null) {
          packages = this.Packages;
        }

        // Add to package list
        // Could be a temporarily list passed in.
        packages.Add(package);
        
      }

      return package;

    }

    /// <summary>
    /// Finds the package name within the package list
    /// </summary>
    /// <param name="packageName">Package Name to find</param>
    /// <returns>Package if found, null if otherwise</returns>
    private IPackage FindPackage(string packageName) {

      if (string.IsNullOrWhiteSpace(packageName)) {
        throw new ArgumentException("Package name cannot be empty");
      }

      return this.Packages.Find(
        p => p.Name.Equals(
          packageName,
          StringComparison.CurrentCultureIgnoreCase
        ));

    }

    /// <summary>
    /// Generates a dependency map
    /// </summary>
    /// <returns>Array of dependency names</returns>
    public string[] GetMap() {

      var map = new List<string>();
     
      foreach (var package in this.Packages) {
        GetPackageDependencies(package, map);
      }

      return map.ToArray();

    }

    /// <summary>
    /// Recursively adds each dependency name in its correct order in the tree.
    /// </summary>
    /// <param name="package">Package</param>
    /// <param name="map">Map List</param>
    private void GetPackageDependencies(IPackage package, IList map) {

      // Recurse through tree if dependency exists
      if (package.Dependency != null) {
        GetPackageDependencies(package.Dependency, map);
      }

      // Don't add if package is already in map list
      if (map.Contains(package.Name)) {
        return;
      }

      // Add the package Name to the list.
      map.Add(package.Name);

    }

    /// <summary>
    /// Determines if the package has a a cycle through recursion
    /// </summary>
    /// <param name="package">Package to check</param>
    /// <param name="originalPackageName">Original Package Name</param>
    /// <returns></returns>
    private bool PackageHasCycle(IPackage package, string originalPackageName) {

      // When dependency is null, can't be a cycle
      if (package.Dependency == null) {
        return false;
      }

      // When package and its dependency have the same name, its a cycle
      if (package.Equals(package.Dependency)) {
        return true;
      }

      // When dependency Name is the same as the original package name, its a cycle
      if (package.Dependency.Name == originalPackageName) {
        return true;
      }

      // Recurse through the dependency tree
      return PackageHasCycle(package.Dependency, originalPackageName);

    }

  }

}
