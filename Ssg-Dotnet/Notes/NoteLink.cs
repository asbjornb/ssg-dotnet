namespace Ssg_Dotnet.Notes;
//Should have preview too shortly. Should also be used for both forward and backward linking
internal record NoteLink(string Url, string Title)
{
    public static NoteLink FromUrl(string Url)
    {
        //Capitalize first letter and replace - with spaces
        var capitalizedTitle = char.ToUpper(Url[0]) + Url[1..].Replace("-", " ");
        return new NoteLink(Url, capitalizedTitle);
    }
}
