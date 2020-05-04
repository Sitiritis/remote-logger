using System;
using Microsoft.AspNetCore.Mvc;
using Misc.Attributes;


namespace AIR.Controllers
{
    [ApiController]
    [ErrorSafe] //todo review
    //[AuthToken]  //todo review
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public class SupportController : Controller
    {
        
        [Route("ping")]
        public IActionResult Ping([FromQuery]string arg)
        {
            Console.WriteLine(arg);
            return Ok(arg);
        }
        
        
        //todo move to LogController
        //todo expand functionality with Tree logging
        [Route("log")]
        [HttpPost]
        public IActionResult Log([FromBody]string message)
        {
            Console.WriteLine(message);
            return Ok();
        }
    }
}