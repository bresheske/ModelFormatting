using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Services
{
    public interface IModelFormatter
    {
        /// <summary>
        /// Standard Formatting.
        /// </summary>
        /// <param name="model">POCO object to be Formatted.</param>
        /// <param name="format">Template string following ModelFormatting
        /// convensions.
        /// Examples:   
        ///     {Name} for 'Name' property.
        ///     {Price:C} for Price property with currency formatting.
        /// </param>
        /// <returns></returns>
        string FormatModel(object model, string format);
    }
}
