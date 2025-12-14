using Microsoft.AspNetCore.Mvc;
using AsyncProgrammingAPI.Services;
using AsyncProgrammingAPI.Models;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AsyncProgrammingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsyncDemoController : ControllerBase
    {
        private readonly IAsyncDemonstrationService asyncService;
        private readonly ILogger<AsyncDemoController> logger;

        public AsyncDemoController(
            IAsyncDemonstrationService asyncService, 
            ILogger<AsyncDemoController> logger)
        {
            this.asyncService = asyncService;
            this.logger = logger;
        }

        /// <summary>
        /// Pokazuje informacje o wątkach przed i po operacji await
        /// </summary>
        [HttpGet("thread-info")]
        public async Task<ActionResult<ThreadInfo>> GetThreadInfo()
        {
            logger.LogInformation("Demonstracja informacji o wątkach");
            
            var result = await asyncService.GetCurrentThreadInfoAsync();
            return Ok(result);
        }

        /// <summary>
        /// Porównuje wydajność operacji synchronicznych vs asynchronicznych
        /// </summary>
        [HttpGet("sync-vs-async")]
        public async Task<ActionResult<ComparisonResult>> CompareSyncVsAsync()
        {
            logger.LogInformation("Porównanie sync vs async");
            
            var result = await asyncService.CompareSync_vs_AsyncAsync();
            return Ok(result);
        }

        /// <summary>
        /// Demonstruje równoległe wykonywanie zadań z Task.WhenAll
        /// </summary>
        [HttpGet("parallel-tasks")]
        public async Task<ActionResult<ParallelTasksResult>> RunParallelTasks()
        {
            logger.LogInformation("Uruchamianie zadań równoległych");
            
            var result = await asyncService.RunParallelTasksAsync();
            return Ok(result);
        }

        /// <summary>
        /// Pokazuje pełny cykl życia Task od utworzenia do zakończenia
        /// </summary>
        [HttpGet("task-lifecycle")]
        public async Task<ActionResult<TaskLifecycleResult>> DemonstrateTaskLifecycle()
        {
            logger.LogInformation("Demonstracja cyklu życia Task");
            
            var result = await asyncService.DemonstrateTaskLifecycleAsync();
            return Ok(result);
        }

        /// <summary>
        /// Pokazuje jak działa ThreadPool w .NET
        /// </summary>
        [HttpGet("thread-pool")]
        public async Task<ActionResult<ThreadPoolResult>> DemonstrateThreadPool()
        {
            logger.LogInformation("Demonstracja ThreadPool");
            
            var result = await asyncService.DemonstrateThreadPoolAsync();
            return Ok(result);
        }

        /// <summary>
        /// Demonstruje różnice z i bez ConfigureAwait(false)
        /// </summary>
        [HttpGet("configure-await")]
        public async Task<ActionResult<ConfigureAwaitResult>> DemonstrateConfigureAwait()
        {
            logger.LogInformation("Demonstracja ConfigureAwait");
            
            var result = await asyncService.DemonstrateConfigureAwaitAsync();
            return Ok(result);
        }

        /// <summary>
        /// Asynchroniczny stream danych z IAsyncEnumerable
        /// </summary>
        [HttpGet("async-stream")]
        public IAsyncEnumerable<StreamItem> GetAsyncStream()
        {
            logger.LogInformation("Uruchamianie async stream");
            return asyncService.GetAsyncStreamAsync();
        }

        /// <summary>
        /// Demonstruje anulowanie długotrwałych operacji za pomocą CancellationToken
        /// </summary>
        [HttpGet("cancellation-demo")]
        public async Task<ActionResult<CancellationResult>> DemonstrateCancellation(CancellationToken cancellationToken)
        {
            logger.LogInformation("Demonstracja anulowania operacji");
            
            var result = await asyncService.DemonstrateCancellationAsync(cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Pokazuje jak uniknąć deadlocków w async/await
        /// </summary>
        [HttpGet("deadlock-prevention")]
        public async Task<ActionResult<DeadlockDemoResult>> DemonstrateDeadlockPrevention()
        {
            logger.LogInformation("Demonstracja zapobiegania deadlockom");
            
            var result = await asyncService.DemonstrateDeadlockPreventionAsync();
            return Ok(result);
        }

        /// <summary>
        /// Kompleksowa demonstracja wszystkich aspektów async/await
        /// </summary>
        [HttpGet("comprehensive-demo")]
        public async Task<ActionResult<object>> ComprehensiveDemo()
        {
            logger.LogInformation("Uruchamianie kompleksowej demonstracji");

            var results = new
            {
                ThreadInfo = await asyncService.GetCurrentThreadInfoAsync(),
                SyncVsAsync = await asyncService.CompareSync_vs_AsyncAsync(),
                ParallelTasks = await asyncService.RunParallelTasksAsync(),
                TaskLifecycle = await asyncService.DemonstrateTaskLifecycleAsync(),
                ConfigureAwait = await asyncService.DemonstrateConfigureAwaitAsync(),
                DeadlockPrevention = await asyncService.DemonstrateDeadlockPreventionAsync(),
                Summary = new
                {
                    KeyConcepts = new[]
                    {
                        "async/await - nieblokujące operacje asynchroniczne",
                        "Task - reprezentuje asynchroniczną operację",
                        "ThreadPool - zarządzanie wątkami przez .NET",
                        "CancellationToken - anulowanie długotrwałych operacji",
                        "ConfigureAwait(false) - optymalizacja kontekstu",
                        "IAsyncEnumerable - asynchroniczne strumieniowanie danych"
                    },
                    BestPractices = new[]
                    {
                        "Zawsze używaj async/await konsekwentnie",
                        "Unikaj sync-over-async (.Result, .Wait())",
                        "Używaj ConfigureAwait(false) w bibliotekach",
                        "Obsługuj CancellationToken w długich operacjach",
                        "Wykorzystuj Task.WhenAll dla równoległości",
                        "Uważaj na wyjątki w async metodach"
                    }
                }
            };

            return Ok(results);
        }

        /// <summary>
        /// Szczegółowe porównanie sync vs async: wszystkie informacje o wątkach, ThreadPool, czasy wykonania, szczegóły zadań
        /// </summary>
        [HttpGet("sync-vs-async-detailed")]
        public async Task<ActionResult<DetailedComparisonResult>> CompareSyncVsAsyncDetailed()
        {
            logger.LogInformation("Szczegółowe porównanie sync vs async");

            // zbierz snapshot środowiska przed
            var before = CaptureThreadPoolSnapshot("before");

            // parametry testu
            const int operations = 8;
            const int perOperationMs = 500;

            // SYNCHRONICZNE: wykonanie sekwencyjne w tym samym wątku (blokujące)
            var syncResult = RunSynchronousWorkload(operations, perOperationMs);

            // zbierz snapshot po sync
            var afterSync = CaptureThreadPoolSnapshot("afterSync");

            // ASYNCHRONICZNE: uruchomienie równoległych asynchronicznych zadań (Task.Delay)
            var asyncResult = await RunAsynchronousWorkload(operations, perOperationMs);

            // zbierz snapshot po async
            var afterAsync = CaptureThreadPoolSnapshot("afterAsync");

            // zbuduj wynik
            var result = new DetailedComparisonResult
            {
                EnvironmentSnapshot = new EnvironmentSnapshot
                {
                    ProcessorCount = Environment.ProcessorCount,
                    ProcessId = Process.GetCurrentProcess().Id,
                    ThreadCount = Process.GetCurrentProcess().Threads.Count
                },
                ThreadPoolBefore = before,
                ThreadPoolAfterSync = afterSync,
                ThreadPoolAfterAsync = afterAsync,
                Synchronous = syncResult,
                Asynchronous = asyncResult,
                Summary = new[]
                {
                    $"Sync total: {syncResult.TotalElapsed.TotalMilliseconds} ms",
                    $"Async total (when all awaited): {asyncResult.TotalElapsed.TotalMilliseconds} ms",
                    $"Observed distinct managed thread IDs (sync): {syncResult.DistinctThreadIds.Count}",
                    $"Observed distinct managed thread IDs (async): {asyncResult.DistinctThreadIds.Count}"
                }
            };

            return Ok(result);
        }

        // Pomocnicze metody
        private ThreadPoolSnapshot CaptureThreadPoolSnapshot(string tag)
        {
            ThreadPool.GetAvailableThreads(out var workerAvail, out var completionAvail);
            ThreadPool.GetMaxThreads(out var workerMax, out var completionMax);
            ThreadPool.GetMinThreads(out var workerMin, out var completionMin);

            return new ThreadPoolSnapshot
            {
                Tag = tag,
                Timestamp = DateTime.UtcNow,
                WorkerThreadsAvailable = workerAvail,
                CompletionPortThreadsAvailable = completionAvail,
                WorkerThreadsMax = workerMax,
                CompletionPortThreadsMax = completionMax,
                WorkerThreadsMin = workerMin,
                CompletionPortThreadsMin = completionMin
            };
        }

        private OperationGroupResult RunSynchronousWorkload(int operations, int perOperationMs)
        {
            var swGroup = Stopwatch.StartNew();
            var details = new List<OperationDetail>();
            var threadIds = new HashSet<int>();

            for (int i = 0; i < operations; i++)
            {
                var sw = Stopwatch.StartNew();
                var startThread = Thread.CurrentThread.ManagedThreadId;
                threadIds.Add(startThread);

                // blokująca operacja (symulacja I/O lub CPU)
                Thread.Sleep(perOperationMs);

                sw.Stop();
                details.Add(new OperationDetail
                {
                    Index = i,
                    StartTimeUtc = DateTime.UtcNow - sw.Elapsed,
                    EndTimeUtc = DateTime.UtcNow,
                    Elapsed = sw.Elapsed,
                    StartThreadId = startThread,
                    EndThreadId = Thread.CurrentThread.ManagedThreadId,
                    Notes = "Blocking (Thread.Sleep) executed synchronously"
                });
            }

            swGroup.Stop();

            return new OperationGroupResult
            {
                Operations = details,
                TotalElapsed = swGroup.Elapsed,
                DistinctThreadIds = threadIds.ToList()
            };
        }

        private async Task<OperationGroupResult> RunAsynchronousWorkload(int operations, int perOperationMs)
        {
            var swGroup = Stopwatch.StartNew();
            var details = new List<OperationDetail>();
            var threadIds = new HashSet<int>();

            // Tworzymy zadania równolegle, każde zadanie rejestruje info przed await i po await
            var tasks = Enumerable.Range(0, operations).Select(async i =>
            {
                var opDetail = new OperationDetail { Index = i };
                var startThread = Thread.CurrentThread.ManagedThreadId;
                opDetail.StartThreadId = startThread;
                opDetail.StartTimeUtc = DateTime.UtcNow;
                lock (threadIds) { threadIds.Add(startThread); }

                var sw = Stopwatch.StartNew();
                // asynchroniczna, nieblokująca operacja
                await Task.Delay(perOperationMs).ConfigureAwait(false);

                sw.Stop();
                opDetail.EndTimeUtc = DateTime.UtcNow;
                opDetail.Elapsed = sw.Elapsed;
                opDetail.EndThreadId = Thread.CurrentThread.ManagedThreadId;
                lock (threadIds) { threadIds.Add(opDetail.EndThreadId); }
                opDetail.Notes = "Async (Task.Delay)";

                lock (details) { details.Add(opDetail); }
            }).ToArray();

            await Task.WhenAll(tasks).ConfigureAwait(false);

            swGroup.Stop();

            return new OperationGroupResult
            {
                Operations = details.OrderBy(d => d.Index).ToList(),
                TotalElapsed = swGroup.Elapsed,
                DistinctThreadIds = threadIds.ToList()
            };
        }
    }
}
