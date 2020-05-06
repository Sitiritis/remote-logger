using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AIR.Models
{
  public class LogData
  {
    public string Level { get; set; }
    public string Message { get; set; }
    public List<ClientLogTag> tags = new List<ClientLogTag>();
  }
}
