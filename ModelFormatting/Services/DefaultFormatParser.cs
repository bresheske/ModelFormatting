using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModelFormatting.Services
{
    public class DefaultFormatParser : IFormatParser
    {
        private const string BRACKET_REGEX = @"\{(?<Key>[^:|}]*):?(?<Format>[^\}]*)\}";

        public MatchCollection FindFormatMatches(string format)
        {
            var patt = new Regex(BRACKET_REGEX);
            return patt.Matches(format);
        }
    }
}
