using System;
using System.Collections.Concurrent;
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
      var remoteLogger = await RemoteLogger.getRemoteLogger(new MetadataProvider());

      Console.WriteLine("Finished");
    }

    // private static class Nested
    // {
    //   public static void throws()
    //   {
    //     Action th = () =>
    //     {
    //       // Console.WriteLine(Environment.StackTrace);
    //
    //       var stackTrace = new System.Diagnostics.StackTrace(true);
    //       var frame = stackTrace.GetFrame(0);
    //       Console.WriteLine(frame?.GetMethod()?.DeclaringType?.FullName);
    //       Console.WriteLine(frame?.GetMethod()?.Name);
    //
    //       void f()
    //       {
    //         var stackTrace = new System.Diagnostics.StackTrace(true);
    //         var frame = stackTrace.GetFrame(0);
    //         Console.WriteLine(frame?.GetMethod()?.DeclaringType?.FullName);
    //         Console.WriteLine(frame?.GetMethod()?.Name);
    //       }
    //
    //       f();
    //     };
    //
    //     Action th2 = () =>
    //     {
    //       var a = new System.Diagnostics.StackTrace(true);
    //       var b = a.GetFrame(0);
    //       Console.WriteLine(b?.GetMethod()?.DeclaringType?.FullName);
    //       Console.WriteLine(b?.GetMethod()?.Name);
    //     };
    //
    //     var anon = new
    //     {
    //       thunk = th
    //     };
    //
    //     anon.thunk.Invoke();
    //     th2.Invoke();
    //     // throw new ArgumentException("Invalid value", "param");
    //   }
    // }
    //
    // static void Main(string[] args)
    // {
    //   // todo test RemoteLogger if need
    //   // Console.WriteLine("Hello World!");
    //
    //   try
    //   {
    //     void f()
    //     {
    //       var stackTrace = new System.Diagnostics.StackTrace(true);
    //       var frame = stackTrace.GetFrame(0);
    //       Console.WriteLine(frame?.GetMethod()?.DeclaringType?.FullName);
    //       Console.WriteLine(frame?.GetMethod()?.Name);
    //     }
    //     f();
    //
    //     Nested.throws();
    //   }
    //   catch (Exception e)
    //   {
    //     Console.WriteLine(e.ToString());
    //   }
    // }
  }
}
