
namespace AsyncProgrammingAPI.Services
{
    public interface IAsyncExplanationService
    {

    }

    public class AsyncExplanationService : IAsyncExplanationService
    {
        private readonly ILogger<AsyncExplanationService> _logger;
        private readonly HttpClient _httpClient;

        public AsyncExplanationService(ILogger<AsyncExplanationService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        // Metody serwisu z wyjaśnieniami dotyczącymi programowania asynchronicznego

        public async Task SomeExplanationMethodAsync()
        {
            // Przykładowa metoda asynchroniczna
            await Task.Delay(1000);
            return;
        }
    }
}
