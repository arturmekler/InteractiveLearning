using System.Collections.Concurrent;
using System.Diagnostics;
using AsyncProgrammingAPI.Models;

namespace AsyncProgrammingAPI.Services
{
    public interface IAsyncDemonstrationService
    {
        Task<ThreadInfo> GetCurrentThreadInfoAsync();
        Task<ComparisonResult> CompareSync_vs_AsyncAsync();
        Task<ParallelTasksResult> RunParallelTasksAsync();
        Task<TaskLifecycleResult> DemonstrateTaskLifecycleAsync();
        Task<ThreadPoolResult> DemonstrateThreadPoolAsync();
        Task<ConfigureAwaitResult> DemonstrateConfigureAwaitAsync();
        IAsyncEnumerable<StreamItem> GetAsyncStreamAsync();
        Task<CancellationResult> DemonstrateCancellationAsync(CancellationToken cancellationToken);
        Task<DeadlockDemoResult> DemonstrateDeadlockPreventionAsync();
    }

    public class AsyncDemonstrationService : IAsyncDemonstrationService
    {
        private readonly ILogger<AsyncDemonstrationService> _logger;
        private readonly HttpClient _httpClient;

        public AsyncDemonstrationService(ILogger<AsyncDemonstrationService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<ThreadInfo> GetCurrentThreadInfoAsync()
        {
            var beforeAwait = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(100); // Symulacja async operacji
            var afterAwait = Thread.CurrentThread.ManagedThreadId;

            return new ThreadInfo
            {
                ThreadBeforeAwait = beforeAwait,
                ThreadAfterAwait = afterAwait,
                IsThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread,
                ThreadName = Thread.CurrentThread.Name ?? "Unnamed",
                Explanation = beforeAwait == afterAwait 
                    ? "Kontynuacja wykonała się na tym samym wątku"
                    : "Kontynuacja została przełączona na inny wątek z ThreadPool"
            };
        }

        public async Task<ComparisonResult> CompareSync_vs_AsyncAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            
            // Operacja synchroniczna - blokuje wątek
            var syncStart = stopwatch.ElapsedMilliseconds;
            Thread.Sleep(1000);
            var syncEnd = stopwatch.ElapsedMilliseconds;
            var syncTime = syncEnd - syncStart;

            // Operacja asynchroniczna - zwalnia wątek
            var asyncStart = stopwatch.ElapsedMilliseconds;
            await Task.Delay(1000);
            var asyncEnd = stopwatch.ElapsedMilliseconds;
            var asyncTime = asyncEnd - asyncStart;

            stopwatch.Stop();

            return new ComparisonResult
            {
                SynchronousTime = syncTime,
                AsynchronousTime = asyncTime,
                TotalTime = stopwatch.ElapsedMilliseconds,
                Explanation = "Thread.Sleep() blokuje wątek całkowicie, Task.Delay() pozwala na obsługę innych operacji"
            };
        }

        public async Task<ParallelTasksResult> RunParallelTasksAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task<TaskResult>>();

            // Uruchom zadania równolegle
            for (int i = 1; i <= 5; i++)
            {
                int taskId = i;
                tasks.Add(SimulateWorkAsync(taskId, Random.Shared.Next(500, 2000)));
            }

            // Czekaj na wszystkie zadania
            var results = await Task.WhenAll(tasks);
            var task1 = Task.Run(() => Console.WriteLine("cos sie dzieje"));
            stopwatch.Stop();

            return new ParallelTasksResult
            {
                Tasks = results.ToList(),
                TotalExecutionTime = stopwatch.ElapsedMilliseconds,
                TaskCount = tasks.Count,
                Explanation = $"Wszystkie {tasks.Count} zadań wykonało się równolegle w czasie {stopwatch.ElapsedMilliseconds}ms"
            };
        }

        public async Task<TaskLifecycleResult> DemonstrateTaskLifecycleAsync()
        {
            var lifecycleSteps = new List<LifecycleStep>();
            
            // Utworzenie Task
            var task = new Task<string>(() =>
            {
                Thread.Sleep(1000);
                return "Task completed";
            });
            
            lifecycleSteps.Add(new LifecycleStep 
            { 
                Step = "Created", 
                Status = task.Status.ToString(),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Timestamp = DateTime.Now
            });

            // Uruchomienie Task
            task.Start();
            lifecycleSteps.Add(new LifecycleStep 
            { 
                Step = "Started", 
                Status = task.Status.ToString(),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Timestamp = DateTime.Now
            });

            // Czekanie na zakończenie
            await Task.Delay(100);
            lifecycleSteps.Add(new LifecycleStep 
            { 
                Step = "Running", 
                Status = task.Status.ToString(),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Timestamp = DateTime.Now
            });

            var result = await task;
            lifecycleSteps.Add(new LifecycleStep 
            { 
                Step = "Completed", 
                Status = task.Status.ToString(),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Timestamp = DateTime.Now
            });

            return new TaskLifecycleResult
            {
                Result = result,
                LifecycleSteps = lifecycleSteps,
                Explanation = "Demonstracja pełnego cyklu życia Task - od Created do Completed"
            };
        }

        public async Task<ThreadPoolResult> DemonstrateThreadPoolAsync()
        {
            var results = new ConcurrentBag<ThreadPoolInfo>();
            var tasks = new List<Task>();

            // Uruchom wiele zadań aby pokazać ThreadPool w działaniu
            for (int i = 0; i < 10; i++)
            {
                int taskId = i;
                tasks.Add(Task.Run(async () =>
                {
                    results.Add(new ThreadPoolInfo
                    {
                        TaskId = taskId,
                        ThreadId = Thread.CurrentThread.ManagedThreadId,
                        IsThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread,
                        StartTime = DateTime.Now
                    });
                    
                    await Task.Delay(Random.Shared.Next(100, 500));
                    
                    results.Add(new ThreadPoolInfo
                    {
                        TaskId = taskId,
                        ThreadId = Thread.CurrentThread.ManagedThreadId,
                        IsThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread,
                        StartTime = DateTime.Now,
                        IsCompletion = true
                    });
                }));
            }

            await Task.WhenAll(tasks);

            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);

            return new ThreadPoolResult
            {
                ThreadInfos = results.OrderBy(r => r.TaskId).ToList(),
                AvailableWorkerThreads = workerThreads,
                AvailableCompletionPortThreads = completionPortThreads,
                MaxWorkerThreads = maxWorkerThreads,
                MaxCompletionPortThreads = maxCompletionPortThreads,
                UniqueThreadsUsed = results.Select(r => r.ThreadId).Distinct().Count(),
                Explanation = "ThreadPool automatycznie zarządza wątkami i może używać tych samych wątków dla różnych zadań"
            };
        }

        public async Task<ConfigureAwaitResult> DemonstrateConfigureAwaitAsync()
        {
            var results = new List<ConfigureAwaitStep>();

            // Bez ConfigureAwait(false)
            var threadBefore1 = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(100);
            var threadAfter1 = Thread.CurrentThread.ManagedThreadId;
            
            results.Add(new ConfigureAwaitStep
            {
                Description = "Bez ConfigureAwait",
                ThreadBefore = threadBefore1,
                ThreadAfter = threadAfter1,
                ContextCaptured = threadBefore1 == threadAfter1
            });

            // Z ConfigureAwait(false)
            var threadBefore2 = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(100).ConfigureAwait(false);
            var threadAfter2 = Thread.CurrentThread.ManagedThreadId;
            
            results.Add(new ConfigureAwaitStep
            {
                Description = "Z ConfigureAwait(false)",
                ThreadBefore = threadBefore2,
                ThreadAfter = threadAfter2,
                ContextCaptured = threadBefore2 == threadAfter2
            });

            return new ConfigureAwaitResult
            {
                Steps = results,
                Explanation = "ConfigureAwait(false) pozwala uniknąć przechwytywania kontekstu synchronizacji"
            };
        }

        public async IAsyncEnumerable<StreamItem> GetAsyncStreamAsync()
        {
            for (int i = 1; i <= 5; i++)
            {
                await Task.Delay(500);
                yield return new StreamItem
                {
                    Id = i,
                    Data = $"Stream item {i}",
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                    Timestamp = DateTime.Now,
                    Explanation = "IAsyncEnumerable pozwala na asynchroniczne generowanie sekwencji danych"
                };
            }
        }

        public async Task<CancellationResult> DemonstrateCancellationAsync(CancellationToken cancellationToken)
        {
            var steps = new List<string>();
            
            try
            {
                for (int i = 1; i <= 10; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    steps.Add($"Krok {i}/10 wykonany");
                    await Task.Delay(1000, cancellationToken);
                }
                
                return new CancellationResult
                {
                    CompletedSteps = steps,
                    WasCancelled = false,
                    Explanation = "Operacja zakończona pomyślnie - nie została anulowana"
                };
            }
            catch (OperationCanceledException)
            {
                return new CancellationResult
                {
                    CompletedSteps = steps,
                    WasCancelled = true,
                    Explanation = $"Operacja anulowana po {steps.Count} krokach"
                };
            }
        }

        public async Task<DeadlockDemoResult> DemonstrateDeadlockPreventionAsync()
        {
            var results = new List<DeadlockScenario>();

            // Scenariusz 1: Potencjalny deadlock (symulowany)
            results.Add(new DeadlockScenario
            {
                ScenarioName = "Sync over Async (niebezpieczne)",
                Description = "Wywołanie .Result lub .Wait() na async metodzie może powodować deadlock",
                IsSafe = false,
                Recommendation = "Używaj await zamiast .Result"
            });

            // Scenariusz 2: Bezpieczne podejście
            var safeResult = await SafeAsyncMethod();
            results.Add(new DeadlockScenario
            {
                ScenarioName = "Proper async/await",
                Description = "Używanie await przez całą ścieżkę wywołań",
                IsSafe = true,
                Result = safeResult,
                Recommendation = "Zawsze używaj async/await konsekwentnie"
            });

            return new DeadlockDemoResult
            {
                Scenarios = results,
                Explanation = "Deadlocki w async/await można uniknąć używając ConfigureAwait(false) lub konsekwentnego async/await"
            };
        }

        // Metody pomocnicze
        private async Task<TaskResult> SimulateWorkAsync(int taskId, int delayMs)
        {
            var startThread = Thread.CurrentThread.ManagedThreadId;
            var startTime = DateTime.Now;
            
            await Task.Delay(delayMs);
            
            var endThread = Thread.CurrentThread.ManagedThreadId;
            var endTime = DateTime.Now;

            return new TaskResult
            {
                TaskId = taskId,
                StartThreadId = startThread,
                EndThreadId = endThread,
                ExecutionTime = delayMs,
                StartTime = startTime,
                EndTime = endTime,
                ThreadSwitched = startThread != endThread
            };
        }

        private async Task<string> SafeAsyncMethod()
        {
            await Task.Delay(100).ConfigureAwait(false);
            return "Operacja bezpieczna";
        }
    }
}
