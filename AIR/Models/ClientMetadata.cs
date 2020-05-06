using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable enable
namespace AIR.Models
{
  public class ClientMetadata: IXmlSerializable
  {
    public string ClientId { get; set; }
    public string? Os { get; set; }

    public XmlSchema GetSchema()
    {
      return null;
    }

    public void ReadXml(XmlReader reader)
    {
      throw new System.NotImplementedException();
    }

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteStartElement("ClientMetadata");
      writer.WriteElementString("ID", ClientId);

      if (Os != null)
      {
        writer.WriteElementString("OS", Os);
      }

      writer.WriteEndElement();
    }
  }
}