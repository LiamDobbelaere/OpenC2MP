using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP {
    internal static class ClientExtensions {
        public static string ToTitleCase(this string text) {
            string[] parts = text.ToLower().Split(' ');

            for (int i = 0; i < parts.Length; i++) {
                parts[i] = parts[i].Substring(0, 1).ToUpper() + parts[i].Substring(1);
            }

            return string.Join(" ", parts);
        }
    }
}
