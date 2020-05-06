using System;
using System.Collections.Immutable;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using AIR.Models;
using Microsoft.Extensions.Logging;

namespace AIR.Logger
{
  public class SessionLog : IXmlSerializable
  {
    public readonly Guid SessionId;
    public readonly ClientMetadata Metadata;
    public readonly DateTime StartDate;
    public readonly TaggedLog RootLog = new TaggedLog(new LogTag("Logs"));

    public DateTime EndDate { get; }
    private DateTime endDate;

    public SessionLog(Guid sessionId, ClientMetadata metadata, DateTime startDate)
    {
      this.SessionId = sessionId;
      this.Metadata = metadata;
      StartDate = startDate;
      EndDate = StartDate;
    }

    public void AddLog(
      LogLevel logLevel,
      IImmutableList<LogTag> tagPath,
      LogEntry log
    )
    {
      endDate = log.Timestamp;
      RootLog.AddLog(logLevel, tagPath, log);
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
      writer.WriteStartElement("SessionLog");
      writer.WriteAttributeString("startDate", StartDate.ToString());
      writer.WriteAttributeString("endDate", EndDate.ToString());
      writer.WriteAttributeString("sessionId", SessionId.ToString());

      Metadata.WriteXml(writer);

      RootLog.WriteXml(writer);

      writer.WriteEndElement();
    }
  }
}
