using System.Diagnostics;
using System.Text;

public class MainVersion
{
    public static void Run()
    {
        Console.WriteLine($"Number of processors: {Environment.ProcessorCount}");
        InnerRun(0, "res1");
        InnerRun(100, "res2");
    }

    static void InnerRun(int msWait, string resultFile)
    {
        Console.WriteLine($"Execute downloads with sleep {msWait} ms");
        const string csvSep = ";";
        StringBuilder csv1 = new("Name" + csvSep);
        for (int i = 1; i <= UrlLoader.Url.Count; i++)
            csv1.Append(i.ToString() + csvSep);
        csv1.Append("\n");
        foreach (var ri in _runnerInfo)
        {
            Console.WriteLine(ri.Description + "...");
            csv1.Append(ri.Description + csvSep);
            ri.Operation(UrlLoader.Url.Count, 0); //ignore first call
            for (int i = 1; i <= UrlLoader.Url.Count; i++)
            {
                List<string?>? res = new();
                //take best time
                TimeSpan ts = new(10, 0, 0);
                for (int j = 0; j < 2; j++)
                {
                    var sw = Stopwatch.StartNew();
                    res = ri.Operation(i, msWait);
                    sw.Stop();
                    if (sw.Elapsed < ts)
                        ts = sw.Elapsed;
                }
                ShowInfo(i, res, ts);
                csv1.Append(Math.Round(ts.TotalSeconds, 1).ToString() + csvSep);
            }
            csv1.Append("\n");
        }
        using StreamWriter writer = new StreamWriter(resultFile + ".csv", false, Encoding.UTF8);
        writer.Write(csv1.ToString());
        Console.WriteLine($"File {resultFile}.csv created");
    }

    record RunnerInfo(string Description, Func<int, int, List<string?>> Operation);
    readonly static List<RunnerInfo> _runnerInfo = [
        new RunnerInfo(
            "Task.Run synchronous task",
            (i, ms) => Executer<string, string?>.Execute_TaskRun(UrlLoader.Url, i, s => 
            { 
                Thread.Sleep(ms);
                return HttpTask.Download(s);
            })
        ),
        new RunnerInfo(
            "Task.Run asynchronous task",
            (i, ms) => Executer<string, string?>.Execute_TaskRunAsync(UrlLoader.Url, i, s =>
            {
                Thread.Sleep(ms);
                return HttpTask.DownloadAsync(s);
            })
        ),
        new RunnerInfo(
            "Wait for multiple asynchronous task",
            (i, ms) => Executer<string, string?>.Execute_BlockAsync(UrlLoader.Url, i, s =>
            {
                Thread.Sleep(ms);
                return HttpTask.DownloadAsync(s);
            })
        ),
        new RunnerInfo(
            "Parallel synchronous call with PLINQ",
            (i, ms) => Executer<string, string?>.Execute_PLINQ(UrlLoader.Url, i, s =>
            {
                Thread.Sleep(ms);
                return HttpTask.Download(s);
            })
        ),
        new RunnerInfo(
            "Parallel synchronous call with Parallel loop",
            (i, ms) => Executer<string, string?>.Execute_Parallel(UrlLoader.Url, i, s =>
            {
                Thread.Sleep(ms);
                return HttpTask.Download(s);
            })
        ),
    ];

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
}