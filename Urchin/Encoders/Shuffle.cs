using System;
using System.Collections;
using Urchin.Extensions.BitArray.Swap;
using Urchin.Interfaces;

namespace Urchin.Encoders
{
    public class Shuffle : IWordEncoder
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
                seedSize = value * 32;
            }
        }

        public int SeedSize => seedSize;

        public BitArray Seed { get; set; }
        public BitArray Entropy { get; set; }

        public BitArray Decode(BitArray word)
        {
            BitArray result = new BitArray(word);
            int size = word.Length;
            int[] random = new int[seedSize / 32];
            Seed.CopyTo(random, 0);
            for (int i = size - 1; i >= 0; i--)
            {
                int r = Math.Abs(random[i]) % size;
                result.Swap(i, r);
            }
            return result;
        }

        public BitArray Encode(BitArray word)
        {
            BitArray result = new BitArray(word);
            int size = word.Length;
            int[] random = new int[seedSize];
            Seed.CopyTo(random, 0);
            for (int i = 0; i < size; i++)
            {
                int r = Math.Abs(random[i]) % size;
                result.Swap(i, r);
            }
            return result;
        }
    }
}
