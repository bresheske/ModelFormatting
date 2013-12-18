using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting
{
    /// <summary>
    /// This class is a container for ModelFormatting POCO objects
    /// which may be reflectively parsed once and ahead of time.
    /// The usage of this container is optional.
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Singleton!
        /// </summary>
        private static Core instance;
        private static Core Instance 
        { 
            get 
            {
                if (instance == null)
                    instance = new Core();
                return instance; 
            }
        }

        private Dictionary<Type, Dictionary<string, string>> TypeStore;

        private Core()
        {
            TypeStore = new Dictionary<Type, Dictionary<string, string>>();
        }

        /// <summary>
        /// Registers and enumerates the properties of a
        /// POCO model. Stores the properties into the container
        /// to be used at a later time.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        public static void RegisterModel<TModel>()
            where TModel : class
        {
            // Already exist? kick out early.
            if (GetPropertyMappings<TModel>() != null)
                return;

            var value = new Dictionary<string, string>();

            // Loop through.
            foreach (var p in typeof(TModel).GetProperties())
            {
                var attribs = p.GetCustomAttributes(typeof(DisplayFormatAttribute), true);
                if (attribs.Any())
                {
                    // Found an attribute, cache it.
                    var keyformat = "{0:" + ((DisplayFormatAttribute)attribs.First()).DataFormatString + "}";
                    value.Add(p.Name, keyformat);
                }
            }

            // Add it if we have any values.
            if (value.Keys.Any())
                Instance.TypeStore.Add(typeof(TModel), value);
        }

        public static Dictionary<string,string> GetPropertyMappings(Type type)
        {
            return Instance.TypeStore.ContainsKey(type)
                ? Instance.TypeStore[type]
                : null;
        }

        public static Dictionary<string,string> GetPropertyMappings<TModel>()
            where TModel : class
        {
            return GetPropertyMappings(typeof(TModel));
        }
    }
}
