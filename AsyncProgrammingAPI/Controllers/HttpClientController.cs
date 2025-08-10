using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HttpClientController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public HttpClientController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("sequential-calls")]
    public async Task<IActionResult> SequentialCalls()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var results = new List<object>();

        // Wywołania sekwencyjne - każde czeka na poprzednie
        var response1 = await _httpClient.GetStringAsync("https://httpbin.org/delay/1");
        results.Add(new { call = 1, time = stopwatch.ElapsedMilliseconds });

        var response2 = await _httpClient.GetStringAsync("https://httpbin.org/delay/1");
        results.Add(new { call = 2, time = stopwatch.ElapsedMilliseconds });

        stopwatch.Stop();

        return Ok(new {
            results,
            totalTime = $"{stopwatch.ElapsedMilliseconds}ms",
            explanation = "Wywołania wykonywane jedno po drugim"
        });
    }

    [HttpGet("parallel-calls")]
    public async Task<IActionResult> ParallelCalls()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Uruchom wszystkie wywołania jednocześnie
        var task1 = _httpClient.GetStringAsync("https://httpbin.org/delay/1");
        var task2 = _httpClient.GetStringAsync("https://httpbin.org/delay/1");
        var task3 = _httpClient.GetStringAsync("https://httpbin.org/delay/1");

        // Czekaj na wszystkie
        await Task.WhenAll(task1, task2, task3);
        
        stopwatch.Stop();

        return Ok(new {
            results = new[] {
                new { call = 1, status = task1.IsCompletedSuccessfully },
                new { call = 2, status = task2.IsCompletedSuccessfully },
                new { call = 3, status = task3.IsCompletedSuccessfully }
            },
            totalTime = $"{stopwatch.ElapsedMilliseconds}ms",
            explanation = "Wszystkie wywołania wykonały się równolegle"
        });
    }
}
