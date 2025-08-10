using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ErrorHandlingController : ControllerBase
{
    [HttpGet("task-exception")]
    public async Task<IActionResult> TaskException()
    {
        try
        {
            await TaskThatThrowsException();
            return Ok("Nie powinno się to wykonać");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new {
                error = ex.Message,
                explanation = "Wyjątki w async/await są przechwytywane normalnie"
            });
        }
    }

    [HttpGet("multiple-tasks-with-exceptions")]
    public async Task<IActionResult> MultipleTasksWithExceptions()
    {
        var tasks = new[]
        {
            SafeTaskAsync("Task 1"),
            FaultyTaskAsync("Task 2"),
            SafeTaskAsync("Task 3")
        };

        try
        {
            var results = await Task.WhenAll(tasks);
            return Ok(results);
        }
        catch (Exception ex)
        {
            // Task.WhenAll rzuca wyjątek gdy jakiekolwiek zadanie się nie powiedzie
            var taskResults = tasks.Select(t => new {
                isCompleted = t.IsCompletedSuccessfully,
                isFaulted = t.IsFaulted,
                exception = t.Exception?.GetBaseException()?.Message
            }).ToArray();

            return Ok(new {
                message = "Niektóre zadania się nie powiodły",
                taskResults,
                explanation = "Task.WhenAll rzuca wyjątek przy pierwszym błędzie"
            });
        }
    }

    private async Task<string> TaskThatThrowsException()
    {
        await Task.Delay(1000);
        throw new InvalidOperationException("To jest przykład wyjątku w async metodzie");
    }

    private async Task<string> SafeTaskAsync(string name)
    {
        await Task.Delay(500);
        return $"{name} - OK";
    }

    private async Task<string> FaultyTaskAsync(string name)
    {
        await Task.Delay(300);
        throw new Exception($"{name} - ERROR");
    }
}
