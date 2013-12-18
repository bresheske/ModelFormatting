using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Services
{
    public class DefaultModelFormatter : BaseModelFormatter
    {
        public DefaultModelFormatter()
            : base(new DefaultFormatParser(), new DefaultKeyFormatPrecedenceProvider(), new DefaultPropertyProvider())
        {

        }
    }
}
