using Microsoft.Build.Framework;
using Microsoft.Build.Execution;
using Microsoft.Build.Construction;
using System;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetCliExtensions.Ref
{
    public class RefAdd : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ProjectFile {get; set; }

        [Required]
        public string PackageName {get; set; }

        [Required]
        public string PackageVersion {get; set; }

        private void AddOrUpdatePackageRef(string packageName, string packageVersion, ProjectRootElement proj)
        {            
            //find existing ones
            var packageRefs = proj.Items.Where(i => i.ItemType == "PackageReference")
                .Where(x => x.Include == packageName)
                .ToList();
            //remove any existing ones
            foreach (var packageRef in packageRefs)
            {
                var parent = packageRef.Parent;
                packageRef.Parent.RemoveChild(packageRef);
                //Remove if empty:
                if (!parent.Children.Any())
                {
                    parent.Parent.RemoveChild(parent);
                }
            }

            //add this one
            proj.AddItem("PackageReference", packageName, new Dictionary<string, string>{{"Version", packageVersion}});
        }

        public override bool Execute()
        {
            // Log.LogMessage(MessageImportance.Normal, "Hello world!");
            // Log.LogMessage(MessageImportance.Normal, ProjectFile == null ? "ProjectFile is NULL" : ProjectFile);
            
            // var globalProperties = new Dictionary<string, string>()
            // {
            //     { "MSBuildExtensionsPath", AppContext.BaseDirectory }
            // };

            // if (!string.IsNullOrWhiteSpace(Configuration))
            // {
            //     globalProperties.Add("Configuration", Configuration);
            // }

            // if (!string.IsNullOrWhiteSpace(Framework))
            // {
            //     globalProperties.Add("TargetFramework", Framework);
            // }

            // var projectInstance = new ProjectInstance(ProjectFile, globalProperties, null);
            // Log.LogMessage(MessageImportance.Normal, projectInstance.FullPath);
            
            var rootElement = ProjectRootElement.Open(ProjectFile);
            AddOrUpdatePackageRef(PackageName, PackageVersion, rootElement);
            rootElement.Save();

            return true;


        }

    }
}
