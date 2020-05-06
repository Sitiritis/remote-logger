using System;
using System.Threading.Tasks;
using LS.Support;
using SimpleJSON;

namespace LoggerTest
{
  class Program
  {
    private class MetadataProvider : IMetadataProvider
    {
      public JSONObject getClientMetadata()
      {
        var metadata = new JSONObject();
        metadata.Add("os", Environment.OSVersion.ToString());
        metadata.Add("clientId", new JSONNumber("1"));

        return metadata;
      }
    }

    static async Task Main(string[] args)
    {
      var remoteLogger = await RemoteLogger.GetRemoteLogger(new MetadataProvider());

      var programMainTag = new LogTag(
      "Program",
        "class",
            new LogTag("Main", "method")
        );

      await remoteLogger.LogInformation(programMainTag, "Hello from client");

      var SomeNestedTag = new LogTag(
          "Level1",
          new LogTag(
            "Level2",
            "type 1",
            new LogTag(
                "Level3"
              )
          )
        );

      await remoteLogger.LogWarning(SomeNestedTag, "Warning!");
      await remoteLogger.LogInformation(SomeNestedTag, "Info");
      await remoteLogger.LogError(SomeNestedTag, "Some error occurred...");

      await remoteLogger.LogInformation(SomeNestedTag, "There can be many entries in one section");

      await remoteLogger.LogInformation("Root level log");

      Console.WriteLine("Finished");
    }
  }
}
