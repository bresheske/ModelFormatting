using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelFormatting.Extensions;

namespace ModelFormatting.Services
{
    public class DefaultModelFormatter : IModelFormatter
    {
        public string FormatModel(object model, string format)
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
    }
}
