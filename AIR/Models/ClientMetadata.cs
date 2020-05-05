using System.ComponentModel.DataAnnotations;

#nullable enable
namespace AIR.Models
{
  public class ClientMetadata
  {
    public string clientId { get; set; }
    public string? os { get; set; }
  }
}