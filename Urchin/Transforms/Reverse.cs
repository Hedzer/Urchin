using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;

namespace Urchin.Transforms
{
    class Reverse : IWordTransformer
    {
        public int WordSize { get; set; }
        public int SeedSize { get; }

        public BitArray Seed { get; set; }
        public BitArray Entropy { get; set; }

        public BitArray Transform(BitArray word)
        {
            BitArray result = new BitArray(word);
            int length = result.Length;
            int mid = (length / 2);
            for (int i = 0; i < mid; i++)
            {
                bool bit = result[i];
                result[i] = result[length - i - 1];
                result[length - i - 1] = bit;
            }
            return result;
        }

        BitArray IWordTransformer.Reverse(BitArray word)
        {
            return Transform(word);
        }
    }
}
