using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace dotnetpacker
{
    class Program
    {
        static void Main(string[] args)
        {            
            ExecuteWithCommandLineApp(args);
        }

        private static void ExecuteWithCommandLineApp(string[] args) {

            var cmd = new CommandLineApplication
            {
                Name = "ref",
                FullName = "References",
                Description = "Manage package and project references"
            };
            cmd.HelpOption("-h|--help");

            cmd.Command("add", c =>
            {
                c.Description = "Add package and project references";
                c.HelpOption("-h|--help");

                var packageName = c.Option("-t|--type <PACKAGE_NAME>",
                    "The package name to install",
                    CommandOptionType.SingleValue);

                var packageVersion = c.Option("-v|--version <PACKAGE_VERSION>",
                    "The package version to install",
                    CommandOptionType.SingleValue);

                c.OnExecute(() =>
                {
                    var msbArguments = $"msbuild /t:Ref /p:PackageName={packageName.Value()} /p:PackageVersion={packageVersion.Value()} /v:m";
                    var psi = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = msbArguments
                    };

                    var process = new Process
                    {
                        StartInfo = psi,
                    };

                    process.Start();

                    return 1;
                });
                
            });

            cmd.Execute(args);

        }
    }
}