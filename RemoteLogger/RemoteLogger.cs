using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using SimpleJSON;

namespace LS.Support
{
  public class RemoteLogger
  {
    private readonly RestClient logClient;

    private RemoteLogger(CookieContainer severCookies, int timeout = 5000)
    {
      logClient = new RestClient(Constants.LogUrl)
      {
        Timeout = timeout,
        CookieContainer = severCookies,
      };
      logClient.AddDefaultHeader("Authorization", $"Bearer {Constants.Token}");
    }

    public static async Task<RemoteLogger> GetRemoteLogger(
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

    public async Task Log(LogLevel logLevel, LogTag tag, string message)
    {
      var request = new RestRequest(Method.POST);

      switch (logLevel)
      {
        case LogLevel.Information:
          break;
        case LogLevel.Warning:
          break;
        case LogLevel.Error:
          break;
        default:
          throw new ArgumentOutOfRangeException(
            nameof(logLevel),
            logLevel,
            $"Remote logger supports only {LogLevel.Information}, {LogLevel.Warning} and {LogLevel.Error} log levels"
          );
      }

      var body = new JSONObject();
      body.Add("level", logLevel.ToString());
      body.Add("message", message);

      if (tag != null)
      {
        body.Add("tags", tag.toJSON());
      }

      request.AddJsonBody(body.ToString());

      var response = await logClient.ExecuteAsync(request);
      if (response.ErrorException != null)
      {
        throw response.ErrorException;
      }
    }

    public async Task Log(LogLevel logLevel, string message)
    {
      await Log(logLevel, null, message);
    }

    public async Task LogInformation(string message)
    {
      await Log(LogLevel.Information, null, message);
    }

    public async Task LogWarning(string message)
    {
      await Log(LogLevel.Warning, null, message);
    }

    public async Task LogError(string message)
    {
      await Log(LogLevel.Error, null, message);
    }

    public async Task LogInformation(LogTag tag, string message)
    {
      await Log(LogLevel.Information, tag, message);
    }

    public async Task LogWarning(LogTag tag, string message)
    {
      await Log(LogLevel.Warning, tag, message);
    }

    public async Task LogError(LogTag tag, string message)
    {
      await Log(LogLevel.Error, tag, message);
    }

    private static class Constants
    {
      public const string
        BaseUrl = "http://localhost:51228/Log"; // not good, but for test task ok; In reality it will be configured in Unity

      public const string
        InitSessionUrl = BaseUrl + "/newSession";

      public const string
        LogUrl = BaseUrl;

      public const string
        Token =
          "QHzNZvsFzqvhy/VcURXzLqrRliFwRJQ+puZQ0a9xBWE="; // not good, but for test task ok; In reality it will be configured in Unity
    }
  }
}