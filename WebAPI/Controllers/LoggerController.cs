using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly IRepoManager _repo;
        private readonly ILoggerManager _logger;
        public LoggerController(IRepoManager repo, ILoggerManager logger)
        {
            _repo = repo;
            _logger = logger;
        }
        [HttpGet]
        public string TestLogger()
        {
            _logger.LogInfo("Here is info message from our values controller.");
            _logger.LogDebug("Here is debug message from our values controller.");
            _logger.LogWarn("Here is warn message from our values controller.");
            _logger.LogError("Here is an error message from our values controller.");
            return "Testing logger success!";
        }
    }
}
