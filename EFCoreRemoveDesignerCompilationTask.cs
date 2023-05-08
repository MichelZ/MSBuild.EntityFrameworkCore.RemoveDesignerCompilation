using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;


namespace MSBuild.EntityFrameworkCore.RemoveDesignerCompilation
{
    public class EFCoreRemoveDesignerCompilationTask : Task
    {
        [Required]
        public string MigrationFilesPath { get; set; }
        [Required]
        public string DBContextNamespace { get; set; }
        public int DesignerFileCountToKeep { get; set; } = 2;
        [Output]
        public string[] LatestDesignerFiles { get; set; }

        public override bool Execute()
        {
            var efCoreFileManipulator = new EFCoreFileManipulator(Log);
            var directory = new DirectoryInfo(MigrationFilesPath);
            efCoreFileManipulator.ProcessDirectory(directory, DBContextNamespace);

            var latestFiles = directory.EnumerateFiles("*.Designer.cs").OrderByDescending(x => x.Name).Take(DesignerFileCountToKeep);
            LatestDesignerFiles = latestFiles.Select(x => x.FullName).ToArray();

            foreach (var file in latestFiles)
            {
                Log.LogMessage(MessageImportance.High, $"RDC: Keeping file {file}");
            }

            Log.LogMessage(MessageImportance.High, $"RDC: Done");
            return !Log.HasLoggedErrors;
        }
    }
}
