using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Services
{
    public class DefaultKeyFormatPrecedenceProvider : IKeyFormatPrecedenceProvider
    {
        public string GetPropertyKeyFormat(object model, string key, string format)
        {
            // Format Precedence Rules 
            var keyformat = "{0}";
            // Highest Precedence: It's cached in the Core.
            var cacheformat = Core.GetPropertyMappings(model.GetType());
            if (cacheformat != null && cacheformat.ContainsKey(key))
                keyformat = cacheformat[key];
            // Precedence: Format is Defined in String. 
            else if (!string.IsNullOrEmpty(format))
                keyformat = "{0:" + format + "}";
            // Precedence: Format is Defined in Annotation.
            else if (model.GetType().GetProperty(key) != null && model.GetType().GetProperty(key)
                .GetCustomAttributes(typeof(DisplayFormatAttribute), true)
                .Any())
            {
                keyformat = "{0:" + ((DisplayFormatAttribute)(model.GetType().GetProperty(key)
                    .GetCustomAttributes(
                        typeof(DisplayFormatAttribute), true).First()))
                            .DataFormatString + "}";
            }

            return keyformat;
        }
    }
}