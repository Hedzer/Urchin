using System;
using System.Collections;
using System.Collections.Generic;
using Urchin.Abstracts;
using Urchin.Interfaces;

namespace Urchin
{
    class Decryptor : CryptoTransform
    {
        public Decryptor(byte[] key, byte[] iv, IKeySchedule keySchedule) : base(key, iv, keySchedule) { }

        public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] block = new byte[inputCount];
            Array.ConstrainedCopy(inputBuffer, inputOffset, block, 0, inputCount);

            //Un-init plans
            List<IWordEncoder> plan =  CreateUninitializationPlan(block);

            // Decode rounds
            BlockEncoder blockEncoder = new BlockEncoder(keySchedule);
            block = blockEncoder.Decode(block, rounds);

            // Un-init block
            BitArray decoded = new BitArray(block);
            plan.ForEach((IWordEncoder decoder) => {
                decoded = decoder.Decode(decoded);
            });
            decoded.CopyTo(block, 0);
            block.CopyTo(outputBuffer, outputOffset);

            return inputCount;
        }

        public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] outputBuffer = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            return outputBuffer;
        }

        protected virtual List<IWordEncoder> CreateUninitializationPlan(byte[] block)
        {
            List<IWordEncoder> decoders = CreateInitialBlockPlan(block.Length);
            decoders.Reverse();
            return decoders;
        }
    }
}
