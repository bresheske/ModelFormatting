using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Services
{
    public interface IKeyFormatPrecedenceProvider
    {
        string GetPropertyKeyFormat(object model, string key, string format);
    }
}
