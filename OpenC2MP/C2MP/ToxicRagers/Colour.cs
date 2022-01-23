﻿// Taken from https://github.com/MaxxWyndham/ToxicRagers with added modifications for .NET 6

namespace C2MP.ToxicRagers {
    public class Colour {
        public float R { get; }

        public float G { get; }

        public float B { get; }

        public float A { get; }

        public static Colour Black => new Colour(0, 0, 0, 1);

        public static Colour White => new Colour(1, 1, 1, 1);

        public Colour(float r, float g, float b, float a) {
            if (r > 1f) { throw new ArgumentOutOfRangeException("r", "r is expected in the 0.0 to 1.0f range"); }
            if (g > 1f) { throw new ArgumentOutOfRangeException("g", "g is expected in the 0.0 to 1.0f range"); }
            if (b > 1f) { throw new ArgumentOutOfRangeException("b", "b is expected in the 0.0 to 1.0f range"); }
            if (a > 1f) { throw new ArgumentOutOfRangeException("a", "a is expected in the 0.0 to 1.0f range"); }

            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static Colour FromArgb(float a, float r, float g, float b) {
            return new Colour(r, g, b, a);
        }

        public static Colour FromArgb(byte a, byte r, byte g, byte b) {
            return new Colour(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        public static Colour FromRgb(byte r, byte g, byte b) {
            return new Colour(r / 255.0f, g / 255.0f, b / 255.0f, 1);
        }

        public static Colour FromColor(Color c) {
            return new Colour(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        public Color ToColor() {
            return Color.FromArgb((int)(A * 255), (int)(R * 255), (int)(G * 255), (int)(B * 255));
        }
    }
}
