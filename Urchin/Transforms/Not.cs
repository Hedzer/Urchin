using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;

namespace Urchin.Transforms
{
    class Not : IWordTransformer
    {
        public int WordSize { get; set; }
        public int SeedSize { get; }

        public BitArray Seed { get; set; }
        public BitArray Entropy { get; set; }

        public BitArray Transform(BitArray word)
        {
            BitArray result = new BitArray(word);
            result.Not();
            return result;
        }

        BitArray Reverse(BitArray word)
        {
            return Transform(word);
        }
    }
}
