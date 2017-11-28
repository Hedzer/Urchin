using System.Collections;
using Urchin.Interfaces;

namespace Urchin.Encoders
{
    public class Not : IWordEncoder
    {
        public int WordSize { get; set; }
        public int SeedSize { get; }

        public BitArray Seed { get; set; }
        public BitArray Entropy { get; set; }

        public BitArray Encode(BitArray word)
        {
            BitArray result = new BitArray(word);
            result.Not();
            return result;
        }

        public BitArray Decode(BitArray word)
        {
            return Encode(word);
        }
    }
}
