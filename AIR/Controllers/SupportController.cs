using System;
using Microsoft.AspNetCore.Mvc;
using Misc.Attributes;


namespace AIR.Controllers
{
  [ApiController]
  [ErrorSafe] // todo review
  // [AuthToken] // todo review
  [ProducesResponseType(200)]
  public class SupportController : ControllerBase
  {
    [Route("ping")]
    public IActionResult Ping([FromQuery] string arg)
    {
      Console.WriteLine(arg);
      return Ok(arg);
    }
  }
}
