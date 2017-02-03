using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error: Missing parameters..");
                Console.WriteLine("     Hint: >MSBuildLessTestRunner.exe \"unitest.dll\" \"msbuild\" ");
                return;
            }

            var targetComponent = args[0];
            var currentCompoentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, targetComponent);
            if (!File.Exists(currentCompoentPath))
            {
                Console.WriteLine("Error: target dll missing or not acceesibale at " + currentCompoentPath);
                return;
            }

            Framework framework;
            if (!Enum.TryParse<Framework>(args[1].ToLower(), out framework))
            {
                Console.WriteLine("Error: Invalid unit test framework.");
                Console.WriteLine("     Supported frameworks: msbuld, xunit and nunit");
                return;
            }

            if (!processConfigFile())
            {
                Console.WriteLine("Error: App.config doesn't exists. Please create one.");
                return;
            }

            TestAbstractRunner testRunner = null;
            switch (framework)
            {
                case Framework.mstest:
                    testRunner = new MSTestRunner();
                    break;

                case Framework.xunit:
                    testRunner = new XUnitTestRunner();
                    break;

                case Framework.nunit:
                    testRunner = new NUnitTestRunner();
                    break;

            }
            var testCases = testRunner.GetTestCases(currentCompoentPath).ToList();
            foreach (var testcase in testCases)
            {
                var log = testcase.Execute();
                Console.WriteLine(log);
            }
        }

        static bool processConfigFile()
        {
            var appConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config");
            if (!File.Exists(appConfigPath))
            {
                return false;
            }
            var testRunnnerConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConsoleTestRunner.exe.config");
            if (File.Exists(testRunnnerConfigPath))
            {
                File.Delete(testRunnnerConfigPath);
            }
            System.IO.File.Move("App.config", "ConsoleTestRunner.exe.config");
            return true;
        }
    }
}
