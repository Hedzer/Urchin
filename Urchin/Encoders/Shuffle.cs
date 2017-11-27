using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;
using Urchin.Extensions.BitArray.Swap;

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
            int[] random = new int[seedSize];
            Seed.CopyTo(random, 0);
            for (int i = wordSize; i > 0; i--)
            {
                int r = Math.Abs(random[i]) % wordSize;
                result.Swap(r, i);
            }
            return result;
        }

        public BitArray Encode(BitArray word)
        {
            BitArray result = new BitArray(word);
            int[] random = new int[seedSize];
            Seed.CopyTo(random, 0);
            for (int i = 0; i < wordSize; i++)
            {
                int r = Math.Abs(random[i]) % wordSize;
                result.Swap(i, r);
            }
            return result;
        }
    }
}
