using Microsoft.AspNetCore.Mvc;
using AsyncProgrammingAPI.Services;
using AsyncProgrammingAPI.Models;

namespace AsyncProgrammingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsyncExplanationController : ControllerBase
    {
        private readonly IAsyncExplanationService asyncExplanationService;
        private readonly ILogger<AsyncExplanationController> logger;

        public AsyncExplanationController(
            IAsyncExplanationService asyncExplanationService,
            ILogger<AsyncExplanationController> logger)
        {
            this.asyncExplanationService = asyncExplanationService;
            this.logger = logger;
        }

                /// </summary>
        [HttpGet("thread-info")]
        public async Task<ActionResult<ThreadInfo>> GetThreadInfo()
        {

            return Ok();
        }

        
    }
}


    