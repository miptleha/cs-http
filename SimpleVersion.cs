using System.Diagnostics;

/// <summary>
/// Testing purpose only
/// Download each url from url.txt synchronous and asynchronous, compare results
/// </summary>
public class SimpleVersion
{
    public static async Task Run()
    {
        int cntOk = 0;
        int cntFail = 0;
        int syncSize = 0;
        int asyncSize = 0;
        TimeSpan tsSync = new TimeSpan();
        TimeSpan tsAsync = new TimeSpan();
        foreach (var url in UrlLoader.Url)
        {
            Console.WriteLine($"Try download url: {url}...");

            var sw = Stopwatch.StartNew();
            string? res1 = HttpTask.Download(url);
            sw.Stop();

            tsSync = tsSync.Add(sw.Elapsed);
            if (res1 != null)
                cntOk++;
            else
                cntFail++;
            syncSize += (res1 ?? "").Length;
            Console.WriteLine($"Done synchronous download, response length: {Nvl(res1?.Length)}, duration: {ToStr(sw.Elapsed)}");

            sw = Stopwatch.StartNew();
            string? res2 = await HttpTask.DownloadAsync(url);
            sw.Stop();

            tsAsync = tsAsync.Add(sw.Elapsed);
            if (res2 == null && res1 != null)
                throw new Exception("Ups... asynchronous download fail, but synchronous not");
            if (res2 != null && res1 == null)
                throw new Exception("Ups... synchronous download fail, but asynchronous not");
            asyncSize += (res2 ?? "").Length;
            Console.WriteLine($"Done asynchronous download, response length: {Nvl(res2?.Length)}, duration: {ToStr(sw.Elapsed)}");

            Console.WriteLine();
        }

        Console.WriteLine($"Ok count: {cntOk}, Fail count: {cntFail}");
        Console.WriteLine($"Synchronous total size: {Math.Round(syncSize / 1024f)} KB, total time: {ToStr(tsSync)}");
        Console.WriteLine($"Asynchronous total size: {Math.Round(asyncSize / 1024f)} KB, total time: {ToStr(tsAsync)}");
    }

    static string ToStr(TimeSpan ts)
    {
        return ts.ToString("mm\\:ss\\.fff");
    }

    static string? Nvl(object? val)
    {
        return val == null ? "-" : val.ToString();
    }
}