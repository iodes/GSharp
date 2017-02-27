using SevenZip;
using System.IO;

namespace GSharp.Compressor
{
    public static class GCompressor
    {
        #region 압축
        public static void Compress(Stream inStream, Stream outStream)
        {
            int dictionary = 1 << 23;
            int posStateBits = 2;
            int litContextBits = 3;
            int litPosBits = 0;
            int algorithm = 2;
            int numFastBytes = 128;

            string mf = "bt4";
            bool eos = true;
            bool stdInMode = false;


            CoderPropID[] propIDs =  {
                CoderPropID.DictionarySize,
                CoderPropID.PosStateBits,
                CoderPropID.LitContextBits,
                CoderPropID.LitPosBits,
                CoderPropID.Algorithm,
                CoderPropID.NumFastBytes,
                CoderPropID.MatchFinder,
                CoderPropID.EndMarker
            };

            object[] properties = {
                dictionary,
                posStateBits,
                litContextBits,
                litPosBits,
                algorithm,
                numFastBytes,
                mf,
                eos
            };

            var encoder = new SevenZip.Compression.LZMA.Encoder();
            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outStream);

            long fileSize;
            if (eos || stdInMode)
            {
                fileSize = -1;
            }
            else
            {
                fileSize = inStream.Length;
            }

            for (int i = 0; i < 8; i++)
            {
                outStream.WriteByte((byte)(fileSize >> (8 * i)));
            }

            encoder.Code(inStream, outStream, -1, -1, null);
        }

        public static byte[] Compress(Stream inStream)
        {
            byte[] output = null;

            using (var outStream = new MemoryStream())
            {
                Compress(inStream, outStream);
                output = outStream.ToArray();
            }

            return output;
        }

        public static void Compress(string input, string output)
        {
            using (var inStream = new FileStream(input, FileMode.Open))
            {
                using (var outStream = new FileStream(output, FileMode.Create))
                {
                    Compress(inStream, outStream);
                }
            }
        }

        public static byte[] Compress(string input)
        {
            byte[] output = null;

            using (var inStream = new FileStream(input, FileMode.Open))
            {
                using (var outStream = new MemoryStream())
                {
                    Compress(inStream, outStream);
                    output = outStream.ToArray();
                }
            }

            return output;
        }
        #endregion

        #region 압축 해제
        public static void Decompress(Stream inStream, Stream outStream)
        {
            var decoder = new SevenZip.Compression.LZMA.Decoder();

            var properties = new byte[5];
            if (inStream.Read(properties, 0, 5) != 5)
            {
                throw new FileLoadException();
            }

            decoder.SetDecoderProperties(properties);

            long outSize = 0;
            for (int i = 0; i < 8; i++)
            {
                int v = inStream.ReadByte();
                if (v < 0)
                {
                    throw new FileLoadException();
                }

                outSize |= ((long)(byte)v) << (8 * i);
            }

            decoder.Code(inStream, outStream, inStream.Length - inStream.Position, outSize, null);
        }

        public static byte[] Decompress(Stream inStream)
        {
            byte[] output = null;

            using (var outStream = new MemoryStream())
            {
                Decompress(inStream, outStream);
                output = outStream.ToArray();
            }

            return output;
        }

        public static void Decompress(string input, string output)
        {
            using (var inStream = new FileStream(input, FileMode.Open))
            {
                using (var outStream = new FileStream(output, FileMode.Create))
                {
                    Decompress(inStream, outStream);
                }
            }
        }

        public static byte[] Decompress(string input)
        {
            byte[] output = null;

            using (var inStream = new FileStream(input, FileMode.Open))
            {
                using (var outStream = new MemoryStream())
                {
                    Decompress(inStream, outStream);
                    output = outStream.ToArray();
                }
            }

            return output;
        }
        #endregion
    }
}
