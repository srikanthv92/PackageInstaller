using PackageInstallerExercise.Packages.Interfaces;

namespace PackageInstallerExercise.Packages.Exceptions {

  public class PackageDuplicateException : PackageExceptionBase {

    public PackageDuplicateException(IPackage package) 
      : base(package) { }

    public override string Name {
      get {
        return "Package already added";
      }
    }

  }
}
