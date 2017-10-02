using PackageInstallerExercise.Packages.Interfaces;

namespace PackageInstallerExercise.Interfaces {

  public interface IDependencyMap {
    IPackage AddPackage(string packageName, string dependencyName);
    string[] GetMap();
  }

}
