/// <summary>
/// Different strategy for parallel execution synchronous and asynchronous methods
/// parallelDegree parameter allow select optimal degree of parallelism
/// </summary>
/// <typeparam name="TParam">parameter type of operation</typeparam>
/// <typeparam name="TResult">output type of operation</typeparam>
public class Executer<TParam, TResult> 
{
    public List<TResult> Execute_TaskRun(List<TParam> data, int parallelDegree, Func<TParam, TResult> operation)
    {
        return new List<TResult>();
    }

    public List<TResult> Execute_TaskRunAsync(List<TParam> data, int parallelDegree, Func<TParam, Task<TResult>> operation)
    {
        return new List<TResult>();
    }

    public List<TResult> Execute_OneThreadAsync(List<TParam> data, int parallelDegree, Func<TParam, Task<TResult>> operation)
    {
        return new List<TResult>();
    }

    public List<TResult> Execute_PLINQ(List<TParam> data, int parallelDegree, Func<TParam, TResult> operation)
    {
        return new List<TResult>();
    }

    public List<TResult> Execute_ParallelFor(List<TParam> data, int parallelDegree, Func<TParam, TResult> operation)
    {
        return new List<TResult>();
    }
}