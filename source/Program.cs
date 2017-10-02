

using PackageInstallerExercise.Interfaces;
using PackageInstallerExercise.Packages;
using PackageInstallerExercise.Types;
using System;

namespace PackageInstallerExercise {

  public class Program {


    private IOutputWriter _writer;
    private IDependencyMapGenerator _generator;
    private string[] _definitions;

    public Program(IOutputWriter writer, IDependencyMapGenerator generator) {
     
      _writer = writer;
      _generator = generator;
    }

    public static int Main(string[] args) {

      var dependencyMap = new PackagesDependencyMap<Package>();

 
      var program = new Program(
        new ConsoleOutputWriter(),
        new PackagesDependencyMapGenerator(':', dependencyMap)
      );

      return (int)program.Run(args);

    }

    public ConsoleReturnTypes Run(string[] args) {

      ConsoleReturnTypes result;

      try {

        result = ConsumeArguments(args);

     
        if (result != ConsoleReturnTypes.Success) {
          HandleError(result);
          return result;
        }

        var dependencyMap = _generator.CreateMap(this._definitions);
        WriteLine(string.Join(", ", dependencyMap));

      }
      catch (Exception e) {
        result = ConsoleReturnTypes.Rejected;
        HandleError(result, e.Message);
      }

      return result;

    }

    private void HandleError(ConsoleReturnTypes failureType, string details = null) {

      switch (failureType) {
        case ConsoleReturnTypes.NoArguments:
        case ConsoleReturnTypes.ArgumentsIncorrectFormat:
          WriteLine("Enter a list of dependencies.");
          WriteLine("Usage: \"<package>: <dependency>, ...\"");
          WriteLine("Usage Example: packageinstallerexcercise \"KittenService: CamelCaser, CamelCaser:\"");
          break;

        case ConsoleReturnTypes.TooManyArguments:
          WriteLine("Only provide one argument.");
          break;

        default:
          string line = string.Format(
            "An error occurred: {0}. \nDetails: {1}.",
            Enum.GetName(typeof(ConsoleReturnTypes), failureType),
            details
          );

          WriteLine(line);
          break;

      }

    }

    private ConsoleReturnTypes ConsumeArguments(string[] args) {

     
      if (args.Length == 0) {
        return ConsoleReturnTypes.NoArguments;
      }

     
      if (args.Length > 1) {
        return ConsoleReturnTypes.TooManyArguments;
      }

      var packagesList = args[0];

      if (string.IsNullOrEmpty(packagesList) ||
        !packagesList.Contains(":")) {
        return ConsoleReturnTypes.ArgumentsIncorrectFormat;
      }

      ParsePackagesList(packagesList);

      if (this._definitions.Length == 1 && this._definitions[0] == ":") {
        return ConsoleReturnTypes.ArgumentsIncorrectFormat;
      }

      return ConsoleReturnTypes.Success;

    }
    private void ParsePackagesList(string packagesList) {

      var splitDefinitions = packagesList.Split(',');

      for (int i = 0; i < splitDefinitions.Length; i++) {
        splitDefinitions[i] = splitDefinitions[i].Trim();
      }

      this._definitions = splitDefinitions;

    }

    private void WriteLine(string s) {
      _writer.WriteLine(s);
    }

  }

}