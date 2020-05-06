using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AIR.Logger
{
  public class LogEntry: IXmlSerializable
  {
    public readonly string Text;
    public readonly DateTime Timestamp;

    public LogEntry(string text, DateTime timestamp)
    {
      this.Text = text;
      this.Timestamp = timestamp;
    }

    public XmlSchema GetSchema()
    {
      return null;
    }

    public void ReadXml(XmlReader reader)
    {
      throw new NotImplementedException();
    }

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteStartElement("Log");
      writer.WriteAttributeString("timestamp", Timestamp.ToString());
      writer.WriteString(Text);
      writer.WriteEndElement();
    }
  }
}