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
    public class RefDel : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ProjectFile {get; set; }

        [Required]
        public string PackageName {get; set; }

        private void RemovePackageRef(string packageName, ProjectRootElement proj)
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
        }

        public override bool Execute()
        {            
            var rootElement = ProjectRootElement.Open(ProjectFile);
            RemovePackageRef(PackageName, rootElement);
            rootElement.Save();

            return true;


        }

    }
}
