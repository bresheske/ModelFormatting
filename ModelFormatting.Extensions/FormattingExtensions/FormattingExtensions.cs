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
            var patt = new Regex(@"\{(?<Key>[^:|}]*):?(?<Format>[^\}]*)\}");
            foreach(Match m in patt.Matches(format))
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
    }
}
