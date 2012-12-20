using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelFormatting.Extensions.FormattingExtensions
{
    public static class FormattingExtensions
    {
        public static string DEFAULT_FORMAT = "{Key}: {Value}";
        public static string DEFAULT_DELIMITER = ", ";

        /// <summary>
        /// Extension for Formatting models (objects) into strings.
        /// 
        /// An example would be object {FirstName="Harry", LastName="Potter"} 
        /// with format "Hello, {FirstName} {LastName}, my name is Luna.".
        /// Also simple string formatting works.
        /// {0:C}, 2m would produce $2.00
        /// </summary>
        /// <param name="model"></param>
        /// <param name="format"></param>
        /// <returns>Formatted String</returns>
        public static string FormatModel(this object model, string format)
        {
            var output = format;
            foreach(Match m in FindFormatMatches(format))
            {
                var prop = model.GetType().GetProperty(m.Groups["Key"].Value);
                var propformat = m.Groups["Format"].Value;
                var val = FormatProperty(prop, model, propformat);

                output = output.Replace(m.Captures[0].Value, val);
            }
            return output;
        }

        public static string FormatModelReflective(this object model, string header, string format, string delimiter, string footer)
        {
            var output = new StringBuilder();
            output.Append(header);

            var props = model.GetType().GetProperties().ToList();
            props.ForEach(prop =>
            {
                var val = format.Replace("{Key}", prop.Name).Replace("{Value}", 
                    FormatProperty(prop, model, string.Empty));
                output.Append(val);
                if (prop != props.Last())
                    output.Append(delimiter);
            });

            output.Append(footer);
            return output.ToString();
        }

        public static string FormatModelReflective(this object model, string header, string delimiter, string footer)
        {
            return model.FormatModelReflective(header, DEFAULT_FORMAT, delimiter, footer);
        }

        public static string FormatModelReflective(this object model, string delimiter)
        {
            return model.FormatModelReflective(string.Empty, DEFAULT_FORMAT, delimiter, string.Empty);
        }

        public static string FormatModelReflective(this object model)
        {
            return model.FormatModelReflective(string.Empty, DEFAULT_FORMAT, DEFAULT_DELIMITER, string.Empty);
        }



        private static MatchCollection FindFormatMatches(string format)
        {
            var patt = new Regex(@"\{(?<Key>[^:|}]*):?(?<Format>[^\}]*)\}");
            return patt.Matches(format);
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
            /* Format Precedence Rules */
            var keyformat = "{0}";
            /* Highest Precedence: Format is Defined in String. */
            if (!string.IsNullOrEmpty(format))
                keyformat = "{0:" + format + "}";
            /* Precedence: Format is Defined in Annotation. */
            else if (model.GetType().GetProperty(key) != null
                && model.GetType().GetProperty(key)
                .GetCustomAttributes(typeof(DisplayFormatAttribute), true)
                .Any())
            {
                keyformat = "{0:" + ((DisplayFormatAttribute)(model.GetType().GetProperty(key)
                    .GetCustomAttributes(typeof(DisplayFormatAttribute), true).First()))
                    .DataFormatString + "}";
            }

            return keyformat;
        }
    }
}
