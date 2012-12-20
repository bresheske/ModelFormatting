using System.Text.RegularExpressions;

namespace ModelFormatting.Extensions.FormattingExtensions
{
    public static class FormattingExtensions
    {
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

        public static string FormatModel(this object model, string header, string format, string delimiter, string footer)
        {
            
        }

        public static string FormatModel(this object model, string header, string delimiter, string footer)
        {
            return model.FormatModel(header, "{Key}: {Value}", delimiter, footer);
        }



        private static MatchCollection FindFormatMatches(string format)
        {
            var patt = new Regex(@"\{(?<Key>[^:|}]*):?(?<Format>[^\}]*)\}");
            return patt.Matches(format);
        }
    }
}
