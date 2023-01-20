namespace Ssg_Dotnet.Config;
internal record ConfigRecord(string InputFolder, string OutputFolder, string NoteFolder, string ContentTemplatePath, string NoteTemplatePath) : IConfig;
