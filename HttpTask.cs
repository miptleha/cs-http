
using System.Net;
using System.Text;
using Microsoft.VisualBasic;

class HttpTask
{
    static HttpTask() 
    {
        //content of url.txt
        Url =  new List<string>();
        using var reader = new StreamReader("url.txt", Encoding.UTF8);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith('#'))
                Url.Add(line);
        }
        
        //global HttpClient
        var handler = new HttpClientHandler();
        if (handler.SupportsAutomaticDecompression)
            handler.AutomaticDecompression = DecompressionMethods.All;
        _httpClient = new HttpClient(handler);

        //random
        _rand = new Random();
    }


    static HttpClient _httpClient;
    static Random _rand;
    public static List<string> Url { get; private set; }

    public string? Download(string url)
    {
        string? result = null;
        try
        {
            var request = HttpWebRequest.CreateHttp(url + AddRandom());
            request.AutomaticDecompression = DecompressionMethods.All;
            using var response = request.GetResponse();
            using var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            result = reader.ReadToEnd();
        }
        catch
        {
            //error
        }
        SaveFileAndStatistic(result);
        return result;
    }

    public async Task<string?> DownloadAsync(string url)
    {
        string? result = null;
        try
        {
            result = await _httpClient.GetStringAsync(url + AddRandom());
        }
        catch
        {
            //error
        }
        SaveFileAndStatistic(result);
        return result;
    }

    private void SaveFileAndStatistic(string? result)
    {
        //TODO: save to file, output some to console, update statistic
    }

    private string AddRandom()
    {
        return "?rand=" + _rand.Next();
    }
}