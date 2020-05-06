using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AIR.Logger;

namespace AIR.Models
{
  public class ClientLogTag
  {
    public string Name { get; set; }
    public string? Type { get; set; }
  }
}