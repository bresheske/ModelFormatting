using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelFormatting.Extensions.FormattingExtensions
{
    public static class FormattingExtensions
    {
        #region Constants

        public const string DEFAULT_FORMAT = "{Key}: {Value}";
        public const string DEFAULT_DELIMITER = ", ";

        #endregion

        #region Normal Formatting

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
            var output = format;
            foreach (Match m in FindFormatMatches(format))
            {
                var prop = model.GetType().GetProperty(m.Groups["Key"].Value);
                if (prop == null)
                    continue;

                var propformat = m.Groups["Format"].Value;
                var val = FormatProperty(prop, model, propformat);

                output = output.Replace(m.Captures[0].Value, val);
            }
            return output;
        }

        #endregion

        #region Reflective Formatting

        /// <summary>
        /// Extension to format an object into a string.
        /// The method uses reflection and looks at properties.
        /// 
        /// Models can use the ComponentModel.DataAnnotations.DisplayFormatAttribute
        /// to define formats for particular properties.
        /// </summary>
        /// <param name="model">
        /// Object to be formatted.
        /// </param>
        /// <param name="format">
        /// String to format each property. Needs to utilize {Key} and {Value}.
        /// Example (xml-style): <{Key}>{Value}</{Key}>
        /// </param>
        /// <param name="delimiter">
        /// String to come between each formatted property.
        /// Common usage is '\n' or '<br />'
        /// </param>
        /// <returns>Formatted string representing the model given.</returns>
        public static string FormatModelReflective(this object model, string format, string delimiter)
        {
            var output = new StringBuilder();

            var props = model.GetType().GetProperties().ToList();
            props.ForEach(prop =>
                {
                    var val = format.Replace("{Key}", prop.Name).Replace("{Value}",
                                                                         FormatProperty(prop, model, string.Empty));
                    output.Append(val);
                    if (prop != props.Last())
                        output.Append(delimiter);
                });

            return output.ToString();
        }

        /// <summary>
        /// Extension to format an object into a string.
        /// The method uses reflection and looks at properties.
        /// 
        /// Models can use the ComponentModel.DataAnnotations.DisplayFormatAttribute
        /// to define formats for particular properties.
        /// </summary>
        /// <param name="model">
        /// Object to be formatted.
        /// </param>
        /// <param name="format">
        /// String to format each property. Needs to utilize {Key} and {Value}.
        /// Example (xml-style): <{Key}>{Value}</{Key}>
        /// </param>
        /// <returns>Formatted string representing the model given.</returns>
        public static string FormatModelReflective(this object model, string format)
        {
            return model.FormatModelReflective(format, string.Empty);
        }

        /// <summary>
        /// Extension to format an object into a string.
        /// The method uses reflection and looks at properties.
        /// 
        /// Models can use the ComponentModel.DataAnnotations.DisplayFormatAttribute
        /// to define formats for particular properties.
        /// </summary>
        /// <param name="model">
        /// Object to be formatted.
        /// </param>
        /// <returns>Formatted string representing the model given.</returns>
        public static string FormatModelReflective(this object model)
        {
            return model.FormatModelReflective(DEFAULT_FORMAT, DEFAULT_DELIMITER);
        }

        #endregion

        #region Private Methods

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
            // Format Precedence Rules 
            var keyformat = "{0}";
            // Highest Precedence: Format is Defined in String. 
            if (!string.IsNullOrEmpty(format))
                keyformat = "{0:" + format + "}";
            // Precedence: Format is Defined in Annotation.
            else if (model.GetType().GetProperty(key) != null
                     && model.GetType().GetProperty(key)
                             .GetCustomAttributes(typeof (DisplayFormatAttribute), true)
                             .Any())
            {
                keyformat = "{0:" + ((DisplayFormatAttribute) (model.GetType().GetProperty(key)
                                                                    .GetCustomAttributes(
                                                                        typeof (DisplayFormatAttribute), true).First()))
                                        .DataFormatString + "}";
            }

            return keyformat;
        }

        #endregion
    }
}
