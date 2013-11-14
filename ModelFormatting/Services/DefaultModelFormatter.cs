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
            return model.FormatModel(format);
        }
    }
}
