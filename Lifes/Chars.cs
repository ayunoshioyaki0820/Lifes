using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifes
{
    internal class Chars
    {
        private static readonly Dictionary<Keys, char> _keyMap = new Dictionary<Keys, char>{
    // 文字キー (シフトなし: 小文字, シフトあり: 大文字)
    { Keys.A, 'a' }, { Keys.B, 'b' }, { Keys.C, 'c' }, { Keys.D, 'd' }, { Keys.E, 'e' },
    { Keys.F, 'f' }, { Keys.G, 'g' }, { Keys.H, 'h' }, { Keys.I, 'i' }, { Keys.J, 'j' },
    { Keys.K, 'k' }, { Keys.L, 'l' }, { Keys.M, 'm' }, { Keys.N, 'n' }, { Keys.O, 'o' },
    { Keys.P, 'p' }, { Keys.Q, 'q' }, { Keys.R, 'r' }, { Keys.S, 's' }, { Keys.T, 't' },
    { Keys.U, 'u' }, { Keys.V, 'v' }, { Keys.W, 'w' }, { Keys.X, 'x' }, { Keys.Y, 'y' },
    { Keys.Z, 'z' },

    // 数字キー (シフトなし: 数字, シフトあり: 特殊文字)
    { Keys.D0, '0' }, { Keys.D1, '1' }, { Keys.D2, '2' }, { Keys.D3, '3' }, { Keys.D4, '4' },
    { Keys.D5, '5' }, { Keys.D6, '6' }, { Keys.D7, '7' }, { Keys.D8, '8' }, { Keys.D9, '9' },

    // テンキー
    { Keys.NumPad0, '0' }, { Keys.NumPad1, '1' }, { Keys.NumPad2, '2' }, { Keys.NumPad3, '3' },
    { Keys.NumPad4, '4' }, { Keys.NumPad5, '5' }, { Keys.NumPad6, '6' }, { Keys.NumPad7, '7' },
    { Keys.NumPad8, '8' }, { Keys.NumPad9, '9' },

    // その他のキー
    { Keys.Space, ' ' },
    { Keys.OemPeriod, '.' },    // . >
    { Keys.OemComma, ',' },     // , <
    { Keys.OemQuestion, '/' },  // / ?
    { Keys.OemSemicolon, ';' }, // ; :
    { Keys.OemQuotes, '\'' },   // ' "
    { Keys.OemOpenBrackets, '[' }, // [ {
    { Keys.OemCloseBrackets, ']' },// ] }
    { Keys.OemPipe, '\\' },     // \ | (OemBackslashの場合もあります)
    { Keys.OemMinus, '-' },     // - _
    { Keys.OemPlus, '=' },      // = +
    { Keys.OemTilde, '`' },     // ` ~
        };
        private static readonly Dictionary<Keys, char> _shiftedKeyMap = new Dictionary<Keys, char>
{
    // 数字キーのシフト
    { Keys.D1, '!' }, { Keys.D2, '@' }, { Keys.D3, '#' }, { Keys.D4, '$' }, { Keys.D5, '%' },
    { Keys.D6, '^' }, { Keys.D7, '&' }, { Keys.D8, '*' }, { Keys.D9, '(' }, { Keys.D0, ')' },

    // その他のキーのシフト
    { Keys.OemPeriod, '>' },
    { Keys.OemComma, '<' },
    { Keys.OemQuestion, '?' },
    { Keys.OemSemicolon, ':' },
    { Keys.OemQuotes, '"' },
    { Keys.OemOpenBrackets, '{' },
    { Keys.OemCloseBrackets, '}' },
    { Keys.OemPipe, '|' },
    { Keys.OemMinus, '_' },
    { Keys.OemPlus, '+' },
    { Keys.OemTilde, '~' },
};
    }
}
