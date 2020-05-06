using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AIR.Logger;
using AIR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Extensions;

namespace AIR.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class LogController : ControllerBase
  {
    private readonly ITaggedLogger<LoggerTag, LogEntry, MemoryStream> logger;

    public LogController(ITaggedLogger<LoggerTag, LogEntry, MemoryStream> taggedLogger)
    {
      logger = taggedLogger;
    }

    [Route("newSession")]
    [HttpPost]
    public async Task<IActionResult> newSession([FromBody] ClientMetadata metadata)
    {
      if (ModelState.IsValid)
      {
        var sessionId = Guid.NewGuid();

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
      else
      {
        return BadRequest();
      }
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Log([FromBody] LogData logData)
    {
      if (ModelState.IsValid)
      {
        if (Request.Cookies.ContainsKey("sessionId"))
        {
          Guid sessionId;

          try
          {
            sessionId = new Guid(Request.Cookies["sessionId"]);
          }
          catch (Exception e)
          {
            return BadRequest("The sessionId is invalid");
          }

          LogLevel logLevel;

          if (logData.Level == LogLevel.Information.ToString())
          {
            logLevel = LogLevel.Information;
          }
          else if (logData.Level == LogLevel.Warning.ToString())
          {
            logLevel = LogLevel.Warning;
          }
          else if (logData.Level == LogLevel.Error.ToString())
          {
            logLevel = LogLevel.Error;
          }
          else
          {
            return BadRequest(new { message = "Invalid log level" });
          }

          await logger.Log(
            logLevel,
            new LoggerTag(
              sessionId,
              logData.tags.Select((tag) => new LogTag(tag.Name, tag.Type)).ToImmutableList()
            ),
            new LogEntry(logData.Message, DateTime.Now)
          );
        }
        else
        {
          return BadRequest("There was no session established");
        }
      }
      else
      {
        return BadRequest();
      }

      return Ok();
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
