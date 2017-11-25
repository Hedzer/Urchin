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
        byte[] Encode(byte[] block);
        // Reverse the transform
        byte[] Decode(byte[] block);
        // Mix, don't repeat the transform
        void PseudoRandomize();
    }
}
