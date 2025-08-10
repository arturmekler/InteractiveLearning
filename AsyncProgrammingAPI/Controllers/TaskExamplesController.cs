using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

[ApiController]
[Route("api/[controller]")]
public class TaskExamplesController : ControllerBase
{
    [HttpGet("task-cancellation")]
    public async Task<IActionResult> TaskCancellation(CancellationToken cancellationToken)
    {
        try
        {
            var result = await LongRunningTaskAsync(cancellationToken);
            return Ok(new { 
                result,
                explanation = "Zadanie zakończone pomyślnie"
            });
        }
        catch (OperationCanceledException)
        {
            return Ok(new { 
                message = "Zadanie zostało anulowane",
                explanation = "CancellationToken pozwala na anulowanie długotrwałych operacji"
            });
        }
    }

    [HttpGet("producer-consumer")]
    public async Task<IActionResult> ProducerConsumer()
    {
        var queue = new ConcurrentQueue<string>();
        var results = new List<string>();

        // Producer - produkuje dane
        var producer = Task.Run(async () =>
        {
            for (int i = 1; i <= 5; i++)
            {
                await Task.Delay(200);
                queue.Enqueue($"Item {i}");
            }
        });

        // Consumer - konsumuje dane
        var consumer = Task.Run(async () =>
        {
            while (!producer.IsCompleted || !queue.IsEmpty)
            {
                if (queue.TryDequeue(out string item))
                {
                    results.Add($"Processed: {item}");
                }
                await Task.Delay(100);
            }
        });

        await Task.WhenAll(producer, consumer);

        return Ok(new {
            processedItems = results,
            explanation = "Wzorzec Producer-Consumer z użyciem ConcurrentQueue"
        });
    }

    [HttpGet("async-enumerable")]
    public IAsyncEnumerable<object> AsyncEnumerable()
    {
        return GenerateDataAsync();
    }

    private async IAsyncEnumerable<object> GenerateDataAsync()
    {
        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(500);
            yield return new { 
                id = i, 
                timestamp = DateTime.Now,
                explanation = "IAsyncEnumerable pozwala na streamowanie danych"
            };
        }
    }

    private async Task<string> LongRunningTaskAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 10; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(1000, cancellationToken);
        }
        return "Zadanie zakończone";
    }
}
