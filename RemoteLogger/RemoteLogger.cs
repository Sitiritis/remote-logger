using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace LS.Support
{
  public class RemoteLogger
  {
    private RemoteLogger(CookieContainer severCookies, int timeout = 5000)
    {
      var logClient = new RestClient(Constants.LogUrl)
      {
        Timeout = timeout,
        CookieContainer = severCookies,
      };
      logClient.AddDefaultHeader("Authorization", $"Bearer {Constants.Token}");
    }

    public static async Task<RemoteLogger> getRemoteLogger(
      IMetadataProvider metadataProvider,
      int establishConnectionTimeout = 5000,
      int logTimeout = 5000
    )
    {
      var initSessionClient =
        new RestClient(Constants.InitSessionUrl)
        {
          Timeout = establishConnectionTimeout,
          CookieContainer = new CookieContainer(),
        };
      initSessionClient.AddDefaultHeader("Authorization", $"Bearer {Constants.Token}");
      var request = new RestRequest(Method.POST);
      request.AddJsonBody(metadataProvider.getClientMetadata().ToString());

      var response = await initSessionClient.ExecuteAsync(request);
      if (response.ErrorException != null)
      {
        throw response.ErrorException;
      }

      return new RemoteLogger(initSessionClient.CookieContainer, logTimeout);
    }

    // ~RemoteLogger()
    // {
    //   // var client = new RestClient(Constants.EndSessionUrl)
    //   // {
    //   //   Timeout = logClient.Timeout,
    //   //   CookieContainer = logClient.CookieContainer,
    //   // };
    //   // var headers = new Dictionary<string, string>(
    //   //   logClient.DefaultParameters
    //   //     .Where(param => param.Type == ParameterType.HttpHeader)
    //   //     .Select(param =>
    //   //       new KeyValuePair<string, string>(param.Name, param.Value.ToString())
    //   //     )
    //   // );
    //   // ;
    //   // client.AddDefaultHeaders(headers);
    //   //
    //   // var request = new RestRequest(Method.POST);
    //   // client.ExecuteAsync(request);
    //   // In this case there is no need to handle the errors. If some troubles
    //   // will happen to the request, the server must timeout with the session.
    // }

    private readonly IRestClient logClient;

    // public async Task Log(LogLevel logLevel, string message)
    // {
    //   switch (logLevel)
    //   {
    //     case LogLevel.Trace:
    //       break;
    //     case LogLevel.Debug:
    //       break;
    //     case LogLevel.Information:
    //       break;
    //     case LogLevel.Warning:
    //       break;
    //     case LogLevel.Error:
    //       break;
    //     case LogLevel.Critical:
    //       break;
    //     case LogLevel.None:
    //       break;
    //     default:
    //       throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, "An unsupported logLevel was passed to the RemoteLogger.Log");
    //   }
    //
    //   // TODO: send request
    // }

    private static class Constants
    {
      public const string
        BaseUrl = "http://localhost:51228/Log"; // not good, but for test task ok; In reality it will be configured in Unity

      public const string
        InitSessionUrl = BaseUrl + "/newSession";

      // public const string
      //   EndSessionUrl = BaseUrl + "/endSession";

      public const string
        LogUrl = BaseUrl; // TODO: does this work fine?

      public const string
        Token =
          "QHzNZvsFzqvhy/VcURXzLqrRliFwRJQ+puZQ0a9xBWE="; // not good, but for test task ok; In reality it will be configured in Unity
    }
  }
}