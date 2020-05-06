using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace AIR.Logger
{
  public class TaggedLogger : ITaggedLogger<LoggerTag, LogEntry, MemoryStream>
  {
    private readonly ConcurrentDictionary<Guid, SessionLog> sessionsLogs =
      new ConcurrentDictionary<Guid, SessionLog>();

    public Task Log(LogLevel logLevel, LoggerTag tag, LogEntry message)
    {
      return Task.Run(() =>
        {
          var sessionLog = sessionsLogs.GetOrAdd(
            tag.SessionId,
            (sessionId) =>
            {
              return new SessionLog(
                sessionId,
                tag.ClientMetadata,
                message.Timestamp
              );
            }
          );

          lock (sessionLog)
          {
            sessionLog.AddLog(logLevel, tag.TagPath, message);
          }
        });
    }

    public async Task<MemoryStream> GetLogs()
    {
      var resultStream = new MemoryStream();

      using (var writer = XmlWriter.Create(
        resultStream,
        new  XmlWriterSettings()
        {
          OmitXmlDeclaration = true,
          CloseOutput = false,
          Async = true
        }
      ))
      {
        writer.WriteStartElement("SessionsLogs");

        foreach (var log in sessionsLogs.Values)
        {
          lock (log)
          {
            log.WriteXml(writer);
          }
        }

        await writer.WriteEndElementAsync();
      }

      resultStream.Position = 0;
      return resultStream;
    }
  }
}