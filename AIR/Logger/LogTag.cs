using System;
using System.Collections.Generic;

#nullable enable
namespace AIR.Logger
{
  public class LogTag: IEquatable<LogTag>
  {
    public readonly string Name;
    public readonly string? Type;

    public LogTag(string name)
    {
      Name = name;
    }

    public LogTag(string name, string type)
    {
      Name = name;
      Type = type;
    }

    public bool Equals(LogTag? other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Name == other.Name && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((LogTag) obj);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Name, Type);
    }
  }
}