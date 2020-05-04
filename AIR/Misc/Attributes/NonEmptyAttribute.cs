using System;
using System.ComponentModel.DataAnnotations;

namespace Misc.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public class NonEmptyAttribute : ValidationAttribute
    {
        public NonEmptyAttribute() : base("Value is required and should be non-empty")
        {
            
        }
        
        
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            // only check string length if empty strings are not allowed
            
            switch (value)
            {
                case Guid guid:
                    return Guid.Empty != guid;
                case string str:
                    return !string.IsNullOrEmpty(str);
                default:
                    return true;
            }
            
        }
    }
}