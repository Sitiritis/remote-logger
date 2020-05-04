using System.ComponentModel.DataAnnotations;

namespace Misc.Attributes
{
    public class GuidAttribute : RegularExpressionAttribute
    {
        public const string GuidRegex = @"^[0-9a-f]{8}-([0-9a-f]{4}\-){3}[0-9a-f]{12}$";

        public GuidAttribute() : base(GuidRegex)
        {
        }
    }

}