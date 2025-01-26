/// <summary>
/// Different strategy for parallel execution synchronous and asynchronous methods
/// Each method allows you to specify the degree of parallelism
/// </summary>
/// <typeparam name="TParam">parameter type of operation</typeparam>
/// <typeparam name="TResult">output type of operation</typeparam>
public class Executer<TParam, TResult>
{
    /// <summary>
    /// Run synchronous operation inside Task.Run n count times, wait for completion, repeat for rest of them
    /// </summary>
    public static List<TResult> Execute_TaskRun(List<TParam> data, int n, Func<TParam, TResult> operation)
    {
        return ExecuteHelper(data, n, (pos) => Task.Run(() => operation(data[pos])));
    }

    /// <summary>
    /// Run asynchronous operation inside Task.Run n count times, wait for completion, repeat for rest of them
    /// </summary>
    public static List<TResult> Execute_TaskRunAsync(List<TParam> data, int n, Func<TParam, Task<TResult>> operation)
    {
        return ExecuteHelper(data, n, (pos) => Task.Run(() => operation(data[pos])));
    }

    /// <summary>
    /// Wait completion of n operation, repeat for rest of them
    /// </summary>
    public static List<TResult> Execute_BlockAsync(List<TParam> data, int n, Func<TParam, Task<TResult>> operation)
    {
        return ExecuteHelper(data, n, (pos) => operation(data[pos]));
    }

    /// <summary>
    /// Execute parallel using PLINQ with n-degree of parallelism
    /// </summary>
    public static List<TResult> Execute_PLINQ(List<TParam> data, int n, Func<TParam, TResult> operation)
    {
        return data.AsParallel()
            .WithDegreeOfParallelism(n)
            .Select(p => operation(p))
            .ToList();
    }

    /// <summary>
    /// Executes in Parallel loop with n-degree of parallelism
    /// </summary>
    public static List<TResult> Execute_Parallel(List<TParam> data, int n, Func<TParam, TResult> operation)
    {
        int n1 = 0;
        int n2 = n < data.Count ? n : data.Count;
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        List<TResult> result = Enumerable.Repeat(default(TResult), data.Count).ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        while (n1 < data.Count)
        {
            Parallel.For(n1, n2, pos => 
            {
                result[pos] = operation(data[pos]);
            });
            n1 = n2;
            n2 = n1 + n < data.Count ? n1 + n : data.Count;
        }
        return result;
    }

    /// <summary>
    /// Helper function for simultaneous asynchronous task execution
    /// </summary>
    private static List<TResult> ExecuteHelper(List<TParam> data, int n, Func<int, Task<TResult>> operation)
    {
        int pos = 0;
        List<TResult> result = new();
        while (pos < data.Count)
        {
            List<Task<TResult>> tasks = new();
            for (int i = 0; i < n && pos < data.Count; i++, pos++)
            {
                tasks.Add(operation(pos));
            }
            var taskBlock = Task.WhenAll(tasks);
            taskBlock.Wait();
            result.AddRange(taskBlock.Result);
        }
        return result;
    }
}