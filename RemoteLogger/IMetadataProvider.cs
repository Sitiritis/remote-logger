using SimpleJSON;

namespace LS.Support
{
  public interface IMetadataProvider
  {
    JSONObject getClientMetadata();
  }
}
