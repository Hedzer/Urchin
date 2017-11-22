using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;
using Urchin.Extensions.BitArray.Reverse;

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
            result.Reverse();
            return result;
        }

        BitArray IWordTransformer.Reverse(BitArray word)
        {
            return Transform(word);
        }
    }
}
