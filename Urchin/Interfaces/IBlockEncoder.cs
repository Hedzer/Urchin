using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urchin.Interfaces
{
    interface IBlockEncoder
    {
        // Key schedule to use as a source of randomness
        IKeySchedule KeySchedule { get; set; }
        // Possible, uninstantiated transformers
        Type[] PossibleTransforms { get; }
        // Instantiated transformers
        IWordEncoder[] Transforms { get; }
        // Transform a block
        byte[] EncodeBlock(byte[] block);
        // Reverse the transform
        byte[] DecodeBlock(byte[] block);
        // Apply transforms iteratively
        byte[] Encode(byte[] block, int iterations);
        // Undo transforms iteratively
        byte[] Decode(byte[] block, int iterations);
        // Mix, don't repeat the transform
        void PseudoRandomize();
    }
}
