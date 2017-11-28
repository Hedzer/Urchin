using System;
using System.Collections;
using Urchin.Interfaces;

namespace Urchin.Encoders
{
    public class Xor : IWordEncoder
    {
        private int wordSize = 0;
        private int seedSize = 0;

        public int WordSize
        {
            get
            {
                return wordSize;
            }
            set
            {
                wordSize = value;
                seedSize = value;
            }
        }

        public int SeedSize => seedSize;

        public BitArray Seed { get; set; }
        public BitArray Entropy { get; set; }

        public BitArray Decode(BitArray word)
        {
            return Encode(word);
        }

        public BitArray Encode(BitArray word)
        {
            BitArray result = new BitArray(word);
            BitArray mix = new BitArray(Seed);
            if (result.Length < mix.Length)
            {
                bool[] shortMix = new bool[mix.Length];
                mix.CopyTo(shortMix, 0);
                Array.Resize<bool>(ref shortMix, result.Length);
                mix = new BitArray(shortMix);
            }
            result.Xor(mix);
            return result;
        }
    }
}
