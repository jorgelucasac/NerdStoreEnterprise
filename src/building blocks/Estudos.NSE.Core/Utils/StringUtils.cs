using System.Linq;

namespace Estudos.NSE.Core.Utils
{
    public static class StringUtils
    {
        public static string ApenasNumeros(this string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }
    }
}