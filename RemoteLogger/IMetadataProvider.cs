using SimpleJSON;

namespace LS.Support
{
  public interface IMetadataProvider
  {
    // TODO: make a separate method for getting the client ID
    JSONObject getClientMetadata();
  }
}