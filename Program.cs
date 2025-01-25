using System.Diagnostics;

//TODO: Call HttpTask.Download and DownloadAsync with different parallel methods
//TODO: In a loop by degree of parallelism measure time of download
//TODO: Output results to plot them in Excel
Console.WriteLine($"Number of processors in system: {Environment.ProcessorCount}");
Console.WriteLine("Task.Run synchronous task...");
Executer<string, string?>.Execute_TaskRun(UrlLoader.Url, UrlLoader.Url.Count, HttpTask.Download);
for (int i = 1; i <= UrlLoader.Url.Count; i += 1+(i/10))
{
    var sw = Stopwatch.StartNew();
    var res = Executer<string, string?>.Execute_TaskRun(UrlLoader.Url, i, HttpTask.Download);
    sw.Stop();
    ShowInfo(i, res, sw.Elapsed);
}

Console.WriteLine("Task.Run asynchronous task...");
Executer<string, string?>.Execute_TaskRunAsync(UrlLoader.Url, UrlLoader.Url.Count, HttpTask.DownloadAsync);
for (int i = 1; i <= UrlLoader.Url.Count; i += 1+(i/10))
{
    var sw = Stopwatch.StartNew();
    var res = Executer<string, string?>.Execute_TaskRunAsync(UrlLoader.Url, i, HttpTask.DownloadAsync);
    sw.Stop();
    ShowInfo(i, res, sw.Elapsed);
}

Console.WriteLine("Wait n asynchronous task...");
Executer<string, string?>.Execute_BlockAsync(UrlLoader.Url, UrlLoader.Url.Count, HttpTask.DownloadAsync);
for (int i = 1; i <= UrlLoader.Url.Count; i += 1+(i/10))
{
    var sw = Stopwatch.StartNew();
    var res = Executer<string, string?>.Execute_BlockAsync(UrlLoader.Url, i, HttpTask.DownloadAsync);
    sw.Stop();
    ShowInfo(i, res, sw.Elapsed);
}

Console.WriteLine("Parallel synchronous call with PLINQ...");
Executer<string, string?>.Execute_PLINQ(UrlLoader.Url, UrlLoader.Url.Count, HttpTask.Download);
for (int i = 1; i <= UrlLoader.Url.Count;  i += 1+(i/10))
{
    var sw = Stopwatch.StartNew();
    var res = Executer<string, string?>.Execute_PLINQ(UrlLoader.Url, i, HttpTask.Download);
    sw.Stop();
    ShowInfo(i, res, sw.Elapsed);
}

Console.WriteLine("Parallel synchronous call with Parallel loop...");
Executer<string, string?>.Execute_Parallel(UrlLoader.Url, UrlLoader.Url.Count, HttpTask.Download);
for (int i = 1; i <= UrlLoader.Url.Count; i += 1+(i/10))
{
    var sw = Stopwatch.StartNew();
    var res = Executer<string, string?>.Execute_Parallel(UrlLoader.Url, i, HttpTask.Download);
    sw.Stop();
    ShowInfo(i, res, sw.Elapsed);
}

//simple version, just check HttpTask
//await SimpleVersion.Run();

static void ShowInfo(int n, List<string?> result, TimeSpan duration)
{
    long size = result.Sum(r => (long)(r ?? "").Length);
    int nOk = result.Count(r => r != null);
    int nFail = result.Count(r => r == null);
    Console.WriteLine(
        $"Parallel degree: {n}, Download size: {SimpleVersion.KB(size)} KB, " +
        $"Success download: {nOk}, Fail download: {nFail}, " +
        $"Duration: {SimpleVersion.ToStr(duration)}"
    );
}
