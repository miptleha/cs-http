using System.Text;

public class UrlLoader
{
    static UrlLoader()
    {
        //content of url.txt
        Url = new List<string>();
        using var reader = new StreamReader("url.txt", Encoding.UTF8);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith('#'))
                Url.Add(line);
        }
    }

    public static List<string> Url { get; private set; }
}