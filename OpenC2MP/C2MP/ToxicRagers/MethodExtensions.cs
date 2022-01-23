// Taken from https://github.com/MaxxWyndham/ToxicRagers with added modifications for .NET 6

namespace C2MP.ToxicRagers {
    public static class MethodExtensions {
        public static string ReadString(this BinaryReader br, int length) {
            if (length == 0) { return null; }

            char[] c = br.ReadChars(length);
            int l = length;

            for (int i = 0; i < length; i++) {
                if (c[i] == 0) {
                    l = i;
                    break;
                }
            }

            return new string(c, 0, l);
        }

        public static void WriteString(this BinaryWriter bw, string s) {
            bw.Write(s.ToCharArray());
        }
    }
}
