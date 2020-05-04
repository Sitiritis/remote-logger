using System;
using RestSharp;

namespace LS.Support
{
    //this is client side implementation; Unity don't like third party dependencies;
    public static class RemoteLogger
    {
        private const string Url = "http://localhost:51228"; //not good, but for test task ok; In reality it will be configured in Unity
        private const string Token = "QHzNZvsFzqvhy/VcURXzLqrRliFwRJQ+puZQ0a9xBWE="; //not good, but for test task ok; In reality it will be configured in Unity


        public static async void Log(Exception e)
        {
            Log(e.ToString());
        }
        
        //todo tag path 
        public static async void Log(string msg)
        {
            try
            {
                var client = new RestClient($"{Url}/log") {Timeout = -1};
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {Token}");
                request.AddJsonBody(msg); //don't use newton.json, only SimpleJSON; trust me else it won't work on some platforms, for example on hololens
                await client.ExecutePostAsync(request);

            }
            catch (Exception e)
            {
                // ignored
            }
        }

    }
}