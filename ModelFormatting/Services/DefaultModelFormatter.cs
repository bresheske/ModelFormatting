using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading;

namespace ModelFormatting.Services
{
    public class DefaultModelFormatter : IModelFormatter
    {
        private readonly IFormatParser formatparser;
        private readonly IKeyFormatPrecedenceProvider keyformatprovider;

        public DefaultModelFormatter(IFormatParser parser, IKeyFormatPrecedenceProvider keyformat)
        {
            formatparser = parser;
            keyformatprovider = keyformat;
        }

        /// <summary>
        /// Formats Teh models!
        /// Override if you wish!
        /// </summary>
        /// <param name="model"></param>
        /// <param name="format"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual string FormatModel(object model, string format, IFormatProvider culture)
        {
            var output = format;
            foreach (Match m in formatparser.FindFormatMatches(format))
            {
                var prop = model.GetType().GetProperty(m.Groups["Key"].Value);
                if (prop == null)
                    continue;

                var propformat = m.Groups["Format"].Value;
                var val = FormatProperty(prop, model, propformat, culture);

                output = output.Replace(m.Captures[0].Value, val);
            }
            return output;
        }

        public string FormatModel(object model, string format)
        {
            return FormatModel(model, format, Thread.CurrentThread.CurrentCulture);
        }

        protected string FormatProperty(PropertyInfo prop, object model, string format, IFormatProvider culture)
        {
            if (prop == null)
                return string.Empty;
            var keyformat = keyformatprovider.GetPropertyKeyFormat(model, prop.Name, format);
            var val = prop.GetValue(model);
            return string.Format(culture, keyformat, val);
        }
    }
}
