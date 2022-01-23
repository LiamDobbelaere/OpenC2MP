// Taken from https://github.com/MaxxWyndham/ToxicRagers with added modifications for .NET 6

namespace C2MP.ToxicRagers {
    public class Palette : List<Colour> {
        public int GetNearestPixelIndex(Colour c) {
            float smallestDiff = float.MaxValue;
            int index = -1;

            for (int i = 0; i < Count; i++) {
                Colour p = this[i];

                float currentDiff = (float)(Math.Pow((c.R - p.R) * 0.299f, 2) + Math.Pow((c.G - p.G) * 0.587f, 2) + Math.Pow((c.B - p.B) * 0.114f, 2));

                if (currentDiff < smallestDiff) {
                    smallestDiff = currentDiff;
                    index = i;
                }
            }

            return index;
        }
    }

}
