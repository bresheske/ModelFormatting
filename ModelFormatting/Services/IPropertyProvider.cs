using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Services
{
    public interface IPropertyProvider
    {
        IEnumerable<PropertyInfo> GetTypeProperties<TModel>()
            where TModel : class;
        IEnumerable<PropertyInfo> GetTypeProperties(Type type);
    }
}
