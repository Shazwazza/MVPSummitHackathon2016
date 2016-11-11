using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace DotNetCliExtensions.Ref
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

                var packageName = c.Argument("[PACKAGE_NAME]", "The package name to install");

                var packageVersion = c.Option("-v|--version <PACKAGE_VERSION>",
                    "The package version to install",
                    CommandOptionType.SingleValue);

                c.OnExecute(() =>
                {
                    var version = "*";
                    if (packageVersion.HasValue()){
                        version = packageVersion.Value();
                    }

                    var msbArguments = $"msbuild /t:RefAdd /p:PackageName={packageName.Value} /p:PackageVersion={version} /v:m";
                    
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

            cmd.Command("del", c =>
            {
                c.Description = "Delete package and project references";
                c.HelpOption("-h|--help");

                var packageName = c.Argument("[PACKAGE_NAME]", "The package name to remove");

                c.OnExecute(() =>
                {                    
                    var msbArguments = $"msbuild /t:RefDel /p:PackageName={packageName.Value} /v:m";
                    
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