using System.ComponentModel.DataAnnotations;

namespace Misc.Attributes
{
    public class MongoIdAttribute:RegularExpressionAttribute
    {
        public const string MongoIdRegex = @"^[0-9a-fA-F]{24}$";

        public MongoIdAttribute() : base(MongoIdRegex)
        {
        }
    }
}