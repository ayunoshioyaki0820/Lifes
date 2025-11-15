using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifes
{
    internal static class Data
    {
        public static Dictionary<string, char> NumbersShift = new Dictionary<string, char>()
        {
            { "1" , '!' },
            { "2" , '\"' },
            { "3" , '#' },
            { "4" , '$' },
            { "5" , '%' },
            { "6" , '&' },
            { "7" , '\'' },
            { "8" , '(' },
            { "9" , ')' },
            { "0" , '0' },
        };
        public static Dictionary<string, char> OemShift = new Dictionary<string, char>()
        {
            { "OemTilde" , '~' },
            { "OemMinus" , '=' },
            { "OemPlus" , '+' },
            { "OemOpenBrackets" , '{' },
            { "OemCloseBrackets" , '}' },
            { "OemPipe" , '|' },
            { "OemSemicolon" , ':' },
            { "OemQuotes" , '\"' },
            { "OemComma" , '<' },
            { "OemPeriod" , '>' },
            { "OemQuestion" , '?' },
        };

        public static Dictionary<string, char> OemNoShift = new Dictionary<string, char>()
        {
            { "OemTilde" , '`' },
            { "OemMinus" , '-' },
            { "OemPlus" , '=' },
            { "OemOpenBrackets" , '[' },
            { "OemCloseBrackets" , ']' },
            { "OemPipe" , '\\' },
            { "OemSemicolon" , ';' },
            { "OemQuotes" , '\'' },
            { "OemComma" , ',' },
            { "OemPeriod" , '.' },
            { "OemQuestion" , '/' },
        };
    }
}
