namespace Ssg_Dotnet.Test.FileSystemReliantTests;

internal static class FileSystemSetup
{
    public const string FolderName = "TestFolder";
    public static List<string> Files = new() { "testFile1.txt", "testFile2.md", "testFile3.json" };

    public static void CreateFiles()
    {
        //Create a few test files in folder "testFiles" for use in tests
        if (!Directory.Exists(FolderName))
        {
            Directory.CreateDirectory(FolderName);
        }
        foreach (var file in Files)
        {
            File.WriteAllText($"{FolderName}/{file}", "SomeText");
        }
    }

    public static void CreateFile(string fileName, string content)
    {
        if (!Directory.Exists(FolderName))
        {
            Directory.CreateDirectory(FolderName);
        }
        File.WriteAllText($"{FolderName}/{fileName}", content);
    }

    public static void CleanUp()
    {
        //Delete the test files
        Directory.Delete(FolderName, true);
    }
}
