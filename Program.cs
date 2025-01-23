//TODO: Call HttpTask.Download and DownloadAsync with different parallel methods
//TODO: In a loop by degree of parallelism measure time of download
//TODO: Output results to plot them in Excel

//simple version, just check HttpTask
using System.Diagnostics;

int cntOk = 0;
int cntFail = 0;
int syncSize = 0;
int asyncSize = 0;
TimeSpan tsSync = new TimeSpan();
TimeSpan tsAsync = new TimeSpan();
foreach (var url in HttpTask.Url)
{
    Console.WriteLine($"Try download url: {url}...");

    HttpTask t1 = new HttpTask();
    var sw = Stopwatch.StartNew();
    string? res1 = t1.Download(url);
    sw.Stop();
    
    tsSync = tsSync.Add(sw.Elapsed);
    if (res1 != null)
        cntOk++;
    else
        cntFail++;
    syncSize += (res1 ?? "").Length;
    Console.WriteLine($"Done synchronous download for {url}, response length: {res1?.Length}, duration: {sw.Elapsed}");

    HttpTask t2 = new HttpTask();
    sw = Stopwatch.StartNew();
    string? res2 = await t2.DownloadAsync(url);
    sw.Stop();
    
    tsAsync = tsAsync.Add(sw.Elapsed);
    if (res2 == null && res1 != null)
        throw new Exception("Ups... asynchronous download fail, but synchronous not");
    if (res2 != null && res1 == null)
        throw new Exception("Ups... synchronous download fail, but asynchronous not");
    asyncSize += (res2 ?? "").Length;
    Console.WriteLine($"Done asynchronous download for {url}, response length: {res1?.Length}, duration: {sw.Elapsed}");

    Console.WriteLine();
}

Console.WriteLine($"Ok count: {cntOk}, Fail count: {cntFail}");
Console.WriteLine($"Synchronous total size: {Math.Round(syncSize / 1024f)} KB, total time: {tsSync}");
Console.WriteLine($"Asynchronous total size: {Math.Round(asyncSize / 1024f)} KB, total time: {tsAsync}");
