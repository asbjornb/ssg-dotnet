namespace Ssg_Dotnet.Config;
internal interface IConfig
{
    string InputFolder { get; }
    string OutputFolder { get; }
    string NoteFolder { get; }
    string ContentTemplatePath { get; }
    string NoteTemplatePath { get; }
}
