using System.Linq;

namespace Kassa.OtherComponents
{
    public static class StringHelper
    {
        public static string TruncateCommas(string str, char comma)
        {
            if (str.Count(ch=> ch == comma) > 1)
            {
                str = Reverse(Reverse(str).Remove(0, Reverse(str).IndexOf(comma) + 1));
                str = TruncateCommas(str, comma);
            }
            return str;
        }
        
        public static string Reverse(string str) => new string(str.ToCharArray().Reverse().ToArray());
    }
}