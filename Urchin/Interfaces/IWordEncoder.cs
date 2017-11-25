using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urchin.Interfaces
{
    interface IWordEncoder
    {
        // How many bits to a word
        int WordSize { get; set; }
        // The seed size in bits needed to do the transofrmation, usually set by WordSize
        int SeedSize { get; }
        // The seed provided for the transformation
        BitArray Seed { get; set; }
        // Additional entropy used to mix bits
        BitArray Entropy { get; set; }
        // Cryptographic transformation in the forward direction
        BitArray Encode(BitArray word);
        // Cryptographic transformation in the reverse direction
        BitArray Decode(BitArray word);
    }
}
