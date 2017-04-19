using System;
using System.Text;

namespace ComPortPackages.Core.Extensions
{
    public static class HexExtensions
    {
        public static string ByteArrayToString(this byte[] ba)
        {
            var hex = new StringBuilder(ba.Length*2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(this string hex)
        {
            var NumberChars = hex.Length;
            var bytes = new byte[NumberChars/2];
            for (var i = 0; i < NumberChars; i += 2)
                bytes[i/2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}