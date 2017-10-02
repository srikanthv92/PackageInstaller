using PackageInstallerExercise.Packages.Interfaces;

namespace PackageInstallerExercise.Packages.Exceptions {
  
  public class PackageContainsCycleException : PackageExceptionBase {

    public PackageContainsCycleException(IPackage package) 
      : base(package) { }

    public override string Name {
      get {
        return "Package Contains Cycle Exception";
      }
    }

  }

}
