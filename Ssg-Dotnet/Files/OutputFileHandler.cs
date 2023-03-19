using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//Small wrapper collecting the functionality I need from System.File, System.Directory and System.Path
namespace Ssg_Dotnet.Files;
internal class OutputFileHandler
{
    private readonly string inputFolder;
    private readonly string outputFolder;

    public OutputFileHandler(string inputFolder, string outputFolder)
    {
        this.inputFolder = inputFolder;
        this.outputFolder = outputFolder;
    }

    public async Task WriteFileAsync(string relativePath, string content)
    {
        var path = GetOutputPath(relativePath);
        CreateSubDirs(path);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        await File.WriteAllTextAsync(path, content);
    }

    //Copy to same location in output
    public void CopyFile(string relativePath)
    {
        var destinationPath = GetOutputPath(relativePath);
        var sourcePath = GetInputPath(relativePath);
        CreateSubDirs(destinationPath);
        File.Copy(sourcePath, destinationPath);
    }

    //Copy to another destination/filename in output
    public void CopyFile(string relativePathInput, string relativeDestinationFolder, string destinationFileName)
    {
        var destinationDirectory = GetOutputPath(relativeDestinationFolder);
        var path = GetInputPath(relativePathInput);
        var destinationPath = Path.Combine(destinationDirectory, destinationFileName);
        CreateSubDirs(destinationPath);
        File.Copy(path, destinationPath);
    }

    public void ClearOutputDirectory()
    {
        if (Directory.Exists(outputFolder))
        {
            Directory.Delete(outputFolder, true);
        }
        Directory.CreateDirectory(outputFolder);
    }

    private string GetInputPath(string relativePath)
    {
        return Path.Combine(inputFolder, relativePath);
    }

    private string GetOutputPath(string relativePath)
    {
        return Path.Combine(outputFolder, relativePath);
    }

    //This might be slow to run for every file we handle
    //Should probably map entire file structure to memory and do this only once for the entire site
    private static void CreateSubDirs(string fullPath)
    {
        //For each folder in path create it if it does not exist
        var folderPath = Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException("Could not get directory from path: " + fullPath);
        var folders = folderPath.Split(Path.DirectorySeparatorChar);
        //Initialize to OS-specific root folder
        var currentPath = Path.GetPathRoot(folderPath) ?? throw new InvalidOperationException("Could not get root path from path: " + folderPath);
        var nonRootfolders = folders.Where(x => x != currentPath);
        foreach (var folder in nonRootfolders)
        {
            currentPath = Path.Combine(currentPath, folder);
            if (!Directory.Exists(currentPath) && !string.IsNullOrEmpty(currentPath))
            {
                var created = Directory.CreateDirectory(currentPath);
            }
        }
    }
}
