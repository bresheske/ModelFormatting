using System;
using System.Collections.Generic;
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

        private Dictionary<Type, IEnumerable<PropertyInfo>> TypeStore;

        private Core()
        {
            TypeStore = new Dictionary<Type, IEnumerable<PropertyInfo>>();
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

            // Loop through.
            Instance.TypeStore.Add(typeof(TModel), typeof(TModel).GetProperties().ToArray());
        }

        public static IEnumerable<PropertyInfo> GetPropertyMappings(Type type)
        {
            return Instance.TypeStore.ContainsKey(type) 
                ? Instance.TypeStore[type]
                : null;
        }

        public static IEnumerable<PropertyInfo> GetPropertyMappings<TModel>()
            where TModel : class
        {
            return GetPropertyMappings(typeof(TModel));
        }
    }
}
