using Microsoft.AspNetCore.Mvc;
using AsyncProgrammingAPI.Services;
using AsyncProgrammingAPI.Models;

namespace AsyncProgrammingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsyncDemoController : ControllerBase
    {
        private readonly IAsyncDemonstrationService _asyncService;
        private readonly ILogger<AsyncDemoController> _logger;

        public AsyncDemoController(
            IAsyncDemonstrationService asyncService, 
            ILogger<AsyncDemoController> logger)
        {
            _asyncService = asyncService;
            _logger = logger;
        }

        /// <summary>
        /// Pokazuje informacje o wątkach przed i po operacji await
        /// </summary>
        [HttpGet("thread-info")]
        public async Task<ActionResult<ThreadInfo>> GetThreadInfo()
        {
            _logger.LogInformation("Demonstracja informacji o wątkach");
            
            var result = await _asyncService.GetCurrentThreadInfoAsync();
            return Ok(result);
        }

        /// <summary>
        /// Porównuje wydajność operacji synchronicznych vs asynchronicznych
        /// </summary>
        [HttpGet("sync-vs-async")]
        public async Task<ActionResult<ComparisonResult>> CompareSyncVsAsync()
        {
            _logger.LogInformation("Porównanie sync vs async");
            
            var result = await _asyncService.CompareSync_vs_AsyncAsync();
            return Ok(result);
        }

        /// <summary>
        /// Demonstruje równoległe wykonywanie zadań z Task.WhenAll
        /// </summary>
        [HttpGet("parallel-tasks")]
        public async Task<ActionResult<ParallelTasksResult>> RunParallelTasks()
        {
            _logger.LogInformation("Uruchamianie zadań równoległych");
            
            var result = await _asyncService.RunParallelTasksAsync();
            return Ok(result);
        }

        /// <summary>
        /// Pokazuje pełny cykl życia Task od utworzenia do zakończenia
        /// </summary>
        [HttpGet("task-lifecycle")]
        public async Task<ActionResult<TaskLifecycleResult>> DemonstrateTaskLifecycle()
        {
            _logger.LogInformation("Demonstracja cyklu życia Task");
            
            var result = await _asyncService.DemonstrateTaskLifecycleAsync();
            return Ok(result);
        }

        /// <summary>
        /// Pokazuje jak działa ThreadPool w .NET
        /// </summary>
        [HttpGet("thread-pool")]
        public async Task<ActionResult<ThreadPoolResult>> DemonstrateThreadPool()
        {
            _logger.LogInformation("Demonstracja ThreadPool");
            
            var result = await _asyncService.DemonstrateThreadPoolAsync();
            return Ok(result);
        }

        /// <summary>
        /// Demonstruje różnice z i bez ConfigureAwait(false)
        /// </summary>
        [HttpGet("configure-await")]
        public async Task<ActionResult<ConfigureAwaitResult>> DemonstrateConfigureAwait()
        {
            _logger.LogInformation("Demonstracja ConfigureAwait");
            
            var result = await _asyncService.DemonstrateConfigureAwaitAsync();
            return Ok(result);
        }

        /// <summary>
        /// Asynchroniczny stream danych z IAsyncEnumerable
        /// </summary>
        [HttpGet("async-stream")]
        public IAsyncEnumerable<StreamItem> GetAsyncStream()
        {
            _logger.LogInformation("Uruchamianie async stream");
            return _asyncService.GetAsyncStreamAsync();
        }

        /// <summary>
        /// Demonstruje anulowanie długotrwałych operacji za pomocą CancellationToken
        /// </summary>
        [HttpGet("cancellation-demo")]
        public async Task<ActionResult<CancellationResult>> DemonstrateCancellation(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Demonstracja anulowania operacji");
            
            var result = await _asyncService.DemonstrateCancellationAsync(cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Pokazuje jak uniknąć deadlocków w async/await
        /// </summary>
        [HttpGet("deadlock-prevention")]
        public async Task<ActionResult<DeadlockDemoResult>> DemonstrateDeadlockPrevention()
        {
            _logger.LogInformation("Demonstracja zapobiegania deadlockom");
            
            var result = await _asyncService.DemonstrateDeadlockPreventionAsync();
            return Ok(result);
        }

        /// <summary>
        /// Kompleksowa demonstracja wszystkich aspektów async/await
        /// </summary>
        [HttpGet("comprehensive-demo")]
        public async Task<ActionResult<object>> ComprehensiveDemo()
        {
            _logger.LogInformation("Uruchamianie kompleksowej demonstracji");

            var results = new
            {
                ThreadInfo = await _asyncService.GetCurrentThreadInfoAsync(),
                SyncVsAsync = await _asyncService.CompareSync_vs_AsyncAsync(),
                ParallelTasks = await _asyncService.RunParallelTasksAsync(),
                TaskLifecycle = await _asyncService.DemonstrateTaskLifecycleAsync(),
                ConfigureAwait = await _asyncService.DemonstrateConfigureAwaitAsync(),
                DeadlockPrevention = await _asyncService.DemonstrateDeadlockPreventionAsync(),
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
    }
}
