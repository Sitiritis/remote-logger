using System;
using System.Collections.Immutable;
using AIR.Models;

namespace AIR.Logger
{
  public class LoggerTag
  {
    public readonly Guid SessionId;
    public readonly IImmutableList<LogTag> TagPath;
    public readonly ClientMetadata? ClientMetadata;

    public LoggerTag(Guid sessionId, IImmutableList<LogTag> tagPath, ClientMetadata? clientMetadata)
    {
      SessionId = sessionId;
      TagPath = tagPath;
      ClientMetadata = clientMetadata;
    }
  }
}