using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Services
{
    public interface IModelFormatter
    {
        string FormatModel(object model, string format, IFormatProvider culture);
        string FormatModel(object model, string format);
    }
}
