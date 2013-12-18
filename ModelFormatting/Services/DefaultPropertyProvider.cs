using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Services
{
    public class DefaultPropertyProvider : IPropertyProvider
    {
        /// <summary>
        /// First checks the core to see if we have registered it. If not, 
        /// enumerates through normally.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public IEnumerable<System.Reflection.PropertyInfo> GetTypeProperties<TModel>() where TModel : class
        {
            return GetTypeProperties(typeof(TModel));
        }

        public IEnumerable<PropertyInfo> GetTypeProperties(Type type)
        {
            return type.GetProperties().ToArray();
        }
    }
}