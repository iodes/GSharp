using SevenZip;
using System.IO;

namespace GSharp.Compressor
{
    public static class GCompressor
    {
        public static void Compress(string input, string output)
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

            using (var inStream = new FileStream(input, FileMode.Open))
            {
                using (var outStream = new FileStream(output, FileMode.Create))
                {
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
            }
        }

        public static void Decompress(string input, string output)
        {
            using (var inStream = new FileStream(input, FileMode.Open))
            {
                using (var outStream = new FileStream(output, FileMode.Create))
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
            }
        }
    }
}
