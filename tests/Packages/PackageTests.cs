using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageInstallerExercise.Packages;

namespace PackageInstallerExercise.Test.Packages {

  [TestClass]
  public class PackageTests {

    [TestMethod]
    [Description("Should return a PackageName:DependencyName string.")]
    public void TestToStringWithDependency() {

      // Arrange
      string packageName = "A",
          dependencyName = "B";

      var package = new Package() {
        Name = packageName,
        Dependency = new Package() {
          Name = dependencyName
        }
      };

      // Act
      var actual = package.ToString();

      // Assert
      Assert.AreEqual(actual, packageName + ":" + dependencyName);

    }

    [TestMethod]
    [Description("Should return a PackageName string.")]
    public void TestToStringWithoutDependency() {

      // Arrange
      string packageName = "A";

      var package = new Package() {
        Name = packageName
      };

      // Act
      var actual = package.ToString();

      // Assert
      Assert.AreEqual(actual, packageName);

    }

    [TestMethod]
    [Description("Should be equal when both packages have same name and dependency.")]
    public void TestEqualSameNameAndDependency() {

      // Arrange
      string packageName = "A",
          dependencyName = "B";

      var package1 = new Package() {
        Name = packageName,
        Dependency = new Package() {
          Name = dependencyName
        }
      };

      var package2 = new Package() {
        Name = packageName,
        Dependency = new Package() {
          Name = dependencyName
        }
      };

      // Act & Assert
      Assert.AreEqual(package1, package2);

    }

    [TestMethod]
    [Description("Should be equal when both packages have same name but different dependencies.")]
    public void TestEqualSameNameDifferentDependency() {

      // Arrange
      string packageName = "A",
          dependencyName = "B";

      var package1 = new Package() {
        Name = packageName,
        Dependency = new Package() {
          Name = dependencyName
        }
      };

      var package2 = new Package() {
        Name = packageName
      };

      // Act & Assert
      Assert.AreEqual(package1, package2);

    }

    [TestMethod]
    [Description("Should not be equal when both packages have same name but different dependencies.")]
    public void TestNotEqual() {

      // Arrange
      var package1 = new Package() {
        Name = "A"
      };

      var package2 = new Package() {
        Name = "B"
      };

      // Act & Assert
      Assert.AreNotEqual(package1, package2);

    }

  }

}
