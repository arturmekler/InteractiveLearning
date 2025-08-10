// Models/AsyncDemoModels.cs
namespace AsyncProgrammingAPI.Models
{
    public record ThreadInfo
    {
        public int ThreadBeforeAwait { get; init; }
        public int ThreadAfterAwait { get; init; }
        public bool IsThreadPoolThread { get; init; }
        public string ThreadName { get; init; } = string.Empty;
        public string Explanation { get; init; } = string.Empty;
    }

    public record ComparisonResult
    {
        public long SynchronousTime { get; init; }
        public long AsynchronousTime { get; init; }
        public long TotalTime { get; init; }
        public string Explanation { get; init; } = string.Empty;
    }

    public record ParallelTasksResult
    {
        public List<TaskResult> Tasks { get; init; } = new();
        public long TotalExecutionTime { get; init; }
        public int TaskCount { get; init; }
        public string Explanation { get; init; } = string.Empty;
    }

    public record TaskResult
    {
        public int TaskId { get; init; }
        public int StartThreadId { get; init; }
        public int EndThreadId { get; init; }
        public int ExecutionTime { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public bool ThreadSwitched { get; init; }
    }

    public record TaskLifecycleResult
    {
        public string Result { get; init; } = string.Empty;
        public List<LifecycleStep> LifecycleSteps { get; init; } = new();
        public string Explanation { get; init; } = string.Empty;
    }

    public record LifecycleStep
    {
        public string Step { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public int ThreadId { get; init; }
        public DateTime Timestamp { get; init; }
    }

    public record ThreadPoolResult
    {
        public List<ThreadPoolInfo> ThreadInfos { get; init; } = new();
        public int AvailableWorkerThreads { get; init; }
        public int AvailableCompletionPortThreads { get; init; }
        public int MaxWorkerThreads { get; init; }
        public int MaxCompletionPortThreads { get; init; }
        public int UniqueThreadsUsed { get; init; }
        public string Explanation { get; init; } = string.Empty;
    }

    public record ThreadPoolInfo
    {
        public int TaskId { get; init; }
        public int ThreadId { get; init; }
        public bool IsThreadPoolThread { get; init; }
        public DateTime StartTime { get; init; }
        public bool IsCompletion { get; init; }
    }

    public record ConfigureAwaitResult
    {
        public List<ConfigureAwaitStep> Steps { get; init; } = new();
        public string Explanation { get; init; } = string.Empty;
    }

    public record ConfigureAwaitStep
    {
        public string Description { get; init; } = string.Empty;
        public int ThreadBefore { get; init; }
        public int ThreadAfter { get; init; }
        public bool ContextCaptured { get; init; }
    }

    public record StreamItem
    {
        public int Id { get; init; }
        public string Data { get; init; } = string.Empty;
        public int ThreadId { get; init; }
        public DateTime Timestamp { get; init; }
        public string Explanation { get; init; } = string.Empty;
    }

    public record CancellationResult
    {
        public List<string> CompletedSteps { get; init; } = new();
        public bool WasCancelled { get; init; }
        public string Explanation { get; init; } = string.Empty;
    }

    public record DeadlockDemoResult
    {
        public List<DeadlockScenario> Scenarios { get; init; } = new();
        public string Explanation { get; init; } = string.Empty;
    }

    public record DeadlockScenario
    {
        public string ScenarioName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public bool IsSafe { get; init; }
        public string? Result { get; init; }
        public string Recommendation { get; init; } = string.Empty;
    }
}
