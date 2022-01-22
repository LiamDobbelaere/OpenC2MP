using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core {
    internal static class Extensions {
        public static bool MatchesTextASCII(this byte[] bytes, int offset, string text) {
            byte[] textBytes = Encoding.ASCII.GetBytes(text);

            for (int i = 0; i < textBytes.Length; i++) {
                if (bytes[offset + i] != textBytes[i]) {
                    return false;
                }
            }

            return true;
        }

        public static bool MatchesOneOfASCII(this byte[] bytes, int offset, string text) {
            byte[] textBytes = Encoding.ASCII.GetBytes(text);

            for (int i = 0; i < textBytes.Length; i++) {
                if (bytes[offset] == textBytes[i]) {
                    return true;
                }
            }

            return false;
        }

        public static string EraseComment(this string configLine) {
            return configLine.Split('/')[0].Trim();
        }
    }
}
