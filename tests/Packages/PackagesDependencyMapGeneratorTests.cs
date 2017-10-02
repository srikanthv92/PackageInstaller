using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageInstallerExercise.Packages;
using System.Collections.Generic;
using PackageInstallerExercise.Interfaces;
using PackageInstallerExercise.Packages.Interfaces;

namespace PackageInstallerExercise.Test {

  [TestClass]
  public class PackagesDependencyMapGeneratorTests {

    private PackagesDependencyMapGenerator generator;
    private PackagesDependencyMapMock dependencyMap;

    [TestInitialize()]
    public void Initialize() {
      dependencyMap = new PackagesDependencyMapMock();
      generator = new PackagesDependencyMapGenerator(':', dependencyMap);
    }

    [TestMethod]
    [Description("Should return a dependency map array list.")]
    public void TestCreateMapReturnsDependencyMapList() {

      // Arrange
      string[] definitions = { "KittenService: CamelCaser", "CamelCaser:" };
      string[] expected = { "CamelCaser", "KittenService" };

      // Act
      var dependencyMapList = generator.CreateMap(definitions);

      // Assert
      CollectionAssert.AreEqual(expected, dependencyMapList);

    }

    [TestMethod]
    [Description("Should fill the packages in the dependency amp.")]
    public void TestCreateMapFillPackagesDependencyMap() {

      // Arrange
      string[] definitions = { "KittenService: CamelCaser", "CamelCaser:" };

      var expectedPackagesAdded = new Dictionary<string, string>() {
        { "KittenService", "CamelCaser" },
        { "CamelCaser", "" }
      };

      // Act
      var dependencyMapList = generator.CreateMap(definitions);

      // Assert
      CollectionAssert.AreEqual(expectedPackagesAdded, dependencyMap.Packages);

    }

  }

  public class PackagesDependencyMapMock : IDependencyMap {

    public Dictionary<string, string> Packages { get; private set; }

    public PackagesDependencyMapMock() {
      this.Packages = new Dictionary<string, string>();
    }

    public IPackage AddPackage(string packageName, string dependencyName) {
      this.Packages.Add(packageName, dependencyName);
      return null;
    }

    public string[] GetMap() {
      return new string[] { "CamelCaser", "KittenService" };
    }

  }

}
