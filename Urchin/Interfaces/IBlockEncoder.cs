using System;
using System.Collections.Generic;
using Urchin.Types;

namespace Urchin.Interfaces
{
    public interface IBlockEncoder
    {
        // Key schedule to use as a source of randomness
        IKeySchedule KeySchedule { get; set; }
        // Possible, uninstantiated transformers
        Type[] PossibleTransforms { get; }
        // Instantiated transformers
        IWordEncoder[] Transforms { get; }
        // Transform a block
        byte[] EncodeBlock(EncodingPlan plan, byte[] block);
        // Undo the transformations
        byte[] DecodeBlock(EncodingPlan plan, byte[] block);
        // Get a snapshot of the round for a given block
        EncodingPlan GetEncodingPlan(int blockLength);
        // Get a snapshot of the round for a given block
        List<EncodingPlan> GetEncodingPlans(int blockLength, int iterations);
        // Apply transforms iteratively
        byte[] Encode(byte[] block, int iterations);
        // Undo transforms iteratively
        byte[] Decode(byte[] block, int iterations);
        // Mix, don't repeat the transform
        void PseudoRandomize();
    }
}
