using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace AIR.Logger
{
  public class TaggedLog: IXmlSerializable
  {
    public readonly LogTag tag;
    public readonly List<LogEntry> Information;
    public readonly List<LogEntry> Warnings;
    public readonly List<LogEntry> Errors;
    public readonly Dictionary<LogTag, TaggedLog> NestedTaggedLogs;

    public TaggedLog(LogTag logTag)
    {
      this.tag = logTag;
      this.Information = new List<LogEntry>();
      this.Warnings = new List<LogEntry>();
      this.Errors = new List<LogEntry>();
      this.NestedTaggedLogs = new Dictionary<LogTag, TaggedLog>();
    }

    public void AddLog(
      LogLevel logLevel,
      IImmutableList<LogTag> tagPath,
      LogEntry log
    )
    {
      if (tagPath.Count > 0)
      {
        var tagToFind = tagPath.First();
        tagPath = tagPath.RemoveAt(0);

        if (NestedTaggedLogs.TryGetValue(tagToFind, out var nextTaggedLog))
        {
          nextTaggedLog.AddLog(logLevel, tagPath, log);
        }
        else
        {
          var newTaggedLog = new TaggedLog(tagToFind);
          NestedTaggedLogs.Add(tagToFind, newTaggedLog);
          newTaggedLog.AddLog(logLevel, tagPath, log);
        }
      }
      else
      {
        switch (logLevel)
        {
          case LogLevel.Information:
            Information.Add(log);
            break;
          case LogLevel.Warning:
            Warnings.Add(log);
            break;
          case LogLevel.Error:
            Errors.Add(log);
            break;
          default:
            throw new ArgumentOutOfRangeException(
              nameof(logLevel),
              logLevel,
              $"Tagged log supports only {LogLevel.Information}, {LogLevel.Warning} and {LogLevel.Error} log levels"
            );
        }
      }
    }

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
      writer.WriteStartElement(tag.Name);

      if (tag.Type != null)
      {
        writer.WriteAttributeString("type", tag.Type);
      }

      writer.WriteStartElement("Information");
      foreach (var info in Information)
      {
        info.WriteXml(writer);
      }
      writer.WriteEndElement();

      writer.WriteStartElement("Warnings");
      foreach (var warning in Warnings)
      {
        warning.WriteXml(writer);
      }
      writer.WriteEndElement();

      writer.WriteStartElement("Errors");
      foreach (var error in Errors)
      {
        error.WriteXml(writer);
      }
      writer.WriteEndElement();

      foreach (var nestedLog in NestedTaggedLogs.Values)
      {
        nestedLog.WriteXml(writer);
      }

      writer.WriteEndElement();
    }
  }
}