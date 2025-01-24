using System.Net;
using System.Text;

/// <summary>
/// Perform GET HTTP Request in synchronous or asynchronous manner
/// Returns content as string (or Task<string>)
/// </summary>
class HttpTask
{
    static HttpTask() 
    {
        //global HttpClient
        var handler = new HttpClientHandler();
        if (handler.SupportsAutomaticDecompression)
            handler.AutomaticDecompression = DecompressionMethods.All;
        _httpClient = new HttpClient(handler);

        //random
        _rand = new Random();
    }

    private HttpTask()
    {}

    static HttpClient _httpClient;
    static Random _rand;

    public static string? Download(string url)
    {
        string? result = null;
        try
        {
            var builder = new UriBuilder(AddRandom(url));
            var request = HttpWebRequest.CreateHttp(builder.Uri);
            request.AutomaticDecompression = DecompressionMethods.All;
            using var response = request.GetResponse();
            using var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            result = reader.ReadToEnd();
        }
        catch
        {
            //error
        }
        return result;
    }

    public static async Task<string?> DownloadAsync(string url)
    {
        string? result = null;
        try
        {
            var builder = new UriBuilder(AddRandom(url));
            result = await _httpClient.GetStringAsync(builder.Uri);
        }
        catch
        {
            //error
            //Console.WriteLine(ex.ToString());
        }
        return result;
    }

    //disable caching
    private static string AddRandom(string url)
    {
        string rnd = "rand=" + _rand.Next();
        string sep = url.IndexOf('?') != -1 ? "&" : "?";
        return $"{url}{sep}{rnd}";
    }
}