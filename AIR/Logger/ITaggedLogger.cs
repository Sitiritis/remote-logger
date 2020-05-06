using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AIR.Logger
{
  public interface ITaggedLogger<in T, in M, R>
  {
    Task Log(LogLevel logLevel, T tag, M message);

    Task<R> GetLogs();
  }
}