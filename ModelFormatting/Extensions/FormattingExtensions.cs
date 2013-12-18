using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelFormatting.Extensions
{
    public static class FormattingExtensions
    {

        /// <summary>
        /// Extension for Formatting models (objects) into strings.
        /// 
        /// An example would be object {FirstName="Harry", LastName="Potter"} 
        /// with format "Hello, {FirstName} {LastName}, my name is Luna.".
        /// Also simple string formatting works.
        /// {Money:C}, 2m would produce $2.00
        /// </summary>
        /// <param name="model"></param>
        /// <param name="format"></param>
        /// <returns>Formatted String</returns>
        public static string FormatModel(this object model, string format)
        {
            
        }


        private static MatchCollection FindFormatMatches(string format)
        {
            
        }

        private static string FormatProperty(PropertyInfo prop, object model, string format)
        {
            if (prop == null)
                return string.Empty;
            var keyformat = GetPropertyKeyFormat(model, prop.Name, format);
            var val = prop.GetValue(model);
            return string.Format(keyformat, val);
        }

        private static string GetPropertyKeyFormat(object model, string key, string format)
        {
            
        }
    }
}
