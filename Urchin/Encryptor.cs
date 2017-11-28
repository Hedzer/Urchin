using System;
using System.Collections;
using Urchin.Abstracts;
using Urchin.Interfaces;

namespace Urchin
{
    class Encryptor : CryptoTransform
    {
        public Encryptor(byte[] key, byte[] iv, IKeySchedule keySchedule) : base(key, iv, keySchedule) {}

        public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] block = new byte[inputCount];
            Array.ConstrainedCopy(inputBuffer, inputOffset, block, 0, inputCount);

            // initialize
            block = InitializeBlock(block);

            // apply rounds
            BlockEncoder blockEncoder = new BlockEncoder(keySchedule);
            block = blockEncoder.Encode(block, rounds);
            block.CopyTo(outputBuffer, outputOffset);

            return inputCount;
        }

        public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] outputBuffer = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            return outputBuffer;
        }

        protected virtual byte[] InitializeBlock(byte[] block)
        {
            byte[] result = new byte[block.Length];
            BitArray encoded = new BitArray(block);
            CreateInitialBlockPlan(block.Length).ForEach((IWordEncoder encoder) => {
                encoded = encoder.Encode(encoded);
            });
            encoded.CopyTo(result, 0);
            return result;
        }

    }
}
