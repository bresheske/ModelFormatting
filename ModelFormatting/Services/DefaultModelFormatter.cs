using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

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

        public virtual string FormatModel(object model, string format)
        {
            var output = format;
            foreach (Match m in formatparser.FindFormatMatches(format))
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

        protected string FormatProperty(PropertyInfo prop, object model, string format)
        {
            if (prop == null)
                return string.Empty;
            var keyformat = keyformatprovider.GetPropertyKeyFormat(model, prop.Name, format);
            var val = prop.GetValue(model);
            return string.Format(keyformat, val);
        }
    }
}
