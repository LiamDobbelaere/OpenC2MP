// Taken from https://github.com/MaxxWyndham/ToxicRagers with added modifications for .NET 6

using C2MP.ToxicRagers;
using System.Text;

namespace C2MP.ToxicRagers {
    public class TWT {
        public string Name { get; set; }

        public string Location { get; set; }

        public List<TWTEntry> Contents { get; set; } = new List<TWTEntry>();

        public static TWT Load(string path) {
            TWT twt;

            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(path))) {
                twt = Load(ms);
            }

            twt.Name = Path.GetFileNameWithoutExtension(path);
            twt.Location = Path.GetDirectoryName(path);

            return twt;
        }

        public static TWT Load(Stream stream) {
            TWT twt = new TWT();

            using (BinaryReader br = new BinaryReader(stream, Encoding.Latin1)) {
                br.ReadInt32();     // length

                int fileCount = br.ReadInt32();

                for (int i = 0; i < fileCount; i++) {
                    twt.Contents.Add(new TWTEntry {
                        Length = br.ReadInt32(),
                        Name = br.ReadString(0x34)
                    });
                }

                for (int i = 0; i < fileCount; i++) {
                    twt.Contents[i].Data = br.ReadBytes(twt.Contents[i].Length);

                    if (twt.Contents[i].Length % 4 > 0) { br.ReadBytes(4 - (twt.Contents[i].Length % 4)); }
                }
            }

            return twt;
        }
    }

    public class TWTEntry {
        public int Length { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }
    }

}
