using PackageInstallerExercise.Interfaces;
using System;

namespace PackageInstallerExercise {

  class ConsoleOutputWriter: IOutputWriter {
    public void WriteLine(string s) {
      Console.WriteLine(s);
    }
  }

}
