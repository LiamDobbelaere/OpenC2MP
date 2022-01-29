using System.Net;
using System.Net.Sockets;
using System.Text;

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

        public static byte[] GetBytes(this string text) {
            return Encoding.ASCII.GetBytes(text);
        }

        public static string EraseComment(this string configLine) {
            return configLine.Split('/')[0].Trim();
        }

        public static int GetPort(this Socket socket) {
            return ((IPEndPoint)socket.LocalEndPoint).Port;
        }
    }
}
