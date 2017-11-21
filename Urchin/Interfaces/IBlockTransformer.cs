using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urchin.Interfaces
{
    interface IBlockTransformer
    {
        // Key schedule to use as a source of randomness
        IKeySchedule KeySchedule { get; set; }
        // Possible, uninstantiated transformers
        Type[] PossibleTransforms { get; }
        // Instantiated transformers
        IWordTransformer[] Transforms { get; }
        // Transform a block
        byte[] Transform(byte[] block);
        // Reverse the transform
        byte[] Reverse(byte[] block);
        // Mix, don't repeat the transform
        void PseudoRandomize();
    }
}
