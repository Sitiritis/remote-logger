using SimpleJSON;

#nullable enable
namespace LS.Support
{
  public class LogTag
  {
    public readonly string Name;
    public readonly string? Type;
    public readonly LogTag? Tag;

    public LogTag(string name, string type, LogTag? logTag)
    {
      Name = name;
      Type = type;
      Tag = logTag;
    }

    public LogTag(string name, LogTag? tag)
    {
      Name = name;
      Tag = tag;
    }

    public LogTag(string name, string type)
    {
      Name = name;
      Type = type;
    }

    public LogTag(string name)
    {
      Name = name;
    }

    public JSONArray toJSON()
    {
      var tagArray = new JSONArray();

      var currentTag = this;
      while (currentTag != null)
      {
        var currentTagJson = new JSONObject();
        currentTagJson.Add("name", currentTag.Name);

        if (currentTag.Type != null)
        {
          currentTagJson.Add("type", currentTag.Type);
        }
        tagArray.Add(currentTagJson);

        currentTag = currentTag.Tag;
      }

      return tagArray;
    }
  }
}