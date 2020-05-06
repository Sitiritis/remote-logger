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
  /// <summary>
  /// Controller responsible for the logging functionality.
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class LogController : ControllerBase
  {
    private readonly ITaggedLogger<LoggerTag, LogEntry, MemoryStream> logger;

    public LogController(ITaggedLogger<LoggerTag, LogEntry, MemoryStream> taggedLogger)
    {
      logger = taggedLogger;
    }

    /// <summary>
    /// Establish a new session with the server. Returns a cookie with the
    /// session id and creates an initial log for the session.
    /// </summary>
    /// <param name="metadata">
    /// Client metadata. clientId field must be present.
    ///
    /// os field is optional.
    /// </param>
    /// <returns>sessionId cookie, which contains a GUID that identifies the session</returns>
    /// <response code="200">The session established successfully.</response>
    /// <response code="400">Wrong metadata.</response>
    [Route("newSession")]
    [HttpPost]
    public async Task<IActionResult> NewSession([FromBody] ClientMetadata metadata)
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

    /// <summary>
    /// Write log the the server.
    /// </summary>
    /// <param name="logData">
    /// Level must either be "Error", "Warning" or "Information".
    ///
    /// Message - string to write in the log.
    ///
    /// Tags - list of objects, that must contain "name" and may optionally
    /// contain "type" fields.
    /// </param>
    /// <returns>
    /// Does not return any value, only reports about the status of the write.
    /// </returns>
    /// <response code="200">The log has successfully been written</response>
    /// <response code="400">Invalid or absent sessionId or log level.</response>
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

    /// <summary>
    /// Get all logs.
    /// </summary>
    /// <returns>Returns the xml, with all logs written to the server as a stream.</returns>
    /// <response code="200"></response>
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
