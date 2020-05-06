using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using AIR.Logger;
using AIR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace AIR.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class LogController : ControllerBase
  {
    private ITaggedLogger<LoggerTag, LogEntry, MemoryStream> logger;

    public LogController(ITaggedLogger<LoggerTag, LogEntry, MemoryStream> taggedLogger)
    {
      this.logger = taggedLogger;
    }

    [Route("newSession")]
    [HttpPost]
    public async Task<IActionResult> newSession([FromBody] ClientMetadata metadata)
    {
      var sessionId = Guid.NewGuid();

      Console.WriteLine($"Session created: {sessionId}"); // TODO: remove

      await logger.Log(
        LogLevel.Information,
        new LoggerTag(
          sessionId,
          ImmutableList<LogTag>.Empty,
          metadata
        ),
        new LogEntry("Session started", DateTime.Now)
      );

      Response.Cookies.Append("sessionId", sessionId.ToString());
      return Ok();
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Log([FromBody] string message)
    {
      // TODO: add the entry to the corresponding log entry

      Console.WriteLine(message);
      return StatusCode(501);
    }

    [Route("")]
    [HttpGet]
    public async Task<FileStreamResult> GetLogs()
    {
      var logsStream = logger.GetLogs();

      return new FileStreamResult(
        await logsStream,
        new MediaTypeHeaderValue("application/xml")
      );
    }
  }
}
