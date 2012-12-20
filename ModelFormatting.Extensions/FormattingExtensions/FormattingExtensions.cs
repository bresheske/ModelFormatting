using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelFormatting.Extensions.FormattingExtensions
{
    public static class FormattingExtensions
    {
        public static string DEFAULT_FORMAT = "{Key}: {Value}";
        public static string DEFAULT_DELIMITER = "\n";

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
                var keyformat = string.IsNullOrEmpty(m.Groups["Format"].Value)
                    ? "{0}"
                    : "{0:" + m.Groups["Format"].Value + "}";

                var val = model.GetType().GetProperty(m.Groups["Key"].Value) != null
                    ? model.GetType().GetProperty(m.Groups["Key"].Value).GetValue(model)
                    : string.Empty;

                output = output.Replace(m.Captures[0].Value, string.Format(keyformat, val));
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
                var tempmodel = new { Key = prop.Name, Value = prop.GetValue(model) };
                output.Append(tempmodel.FormatModel(format));
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
    }
}
