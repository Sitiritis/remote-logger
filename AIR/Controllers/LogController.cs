using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AIR.Models;
using AIR.Models.Log;
using Microsoft.AspNetCore.Mvc;
using Misc.Attributes;

namespace AIR.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class LogController : ControllerBase
  {
    private ConcurrentDictionary<Guid, SessionLog> sessionsLogs =
      new ConcurrentDictionary<Guid, SessionLog>();

    [Route("newSession")]
    [HttpPost]
    public async Task<IActionResult> newSession([FromBody] ClientMetadata metadata)
    {
      var sessionId = Guid.NewGuid();
      sessionsLogs[sessionId] = new SessionLog(metadata);

      Console.WriteLine($"Session created: {sessionId}"); // TODO: remove

      Response.Cookies.Append("sessionId", sessionId.ToString());
      return Ok();
    }

    [Route("")]
    [HttpPost]
    public IActionResult Log([FromBody] string message)
    {
      // TODO: add the entry to the corresponding log entry

      Console.WriteLine(message);
      return StatusCode(501);
    }

    // [Route("endSession")]
    // [HttpPost]
    // public IActionResult endSession()
    // {
    //   Guid sessionId;
    //
    //   try
    //   {
    //     sessionId = new Guid(Request.Cookies["sessionId"]);
    //   }
    //   catch (Exception e)
    //   {
    //     return BadRequest(new
    //     {
    //       message = "The sessionId in cookies is invalid"
    //     });
    //   }
    //
    //   SessionLog sessionLog = null;
    //   if (sessionsLogs.TryRemove(sessionId, out sessionLog))
    //   {
    //     Console.WriteLine($"Session removed: {sessionId}"); // TODO: remove
    //
    //     sessionLog.save();
    //   }
    //   else
    //   {
    //     return NotFound(new
    //     {
    //       message = $"The {sessionId} session was is not active"
    //     });
    //   }
    //
    //   return Ok();
    // }
  }
}
