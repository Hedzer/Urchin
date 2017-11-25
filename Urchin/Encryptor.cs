using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Urchin.Interfaces;
using Urchin.Transforms;
using System.Collections;
using Urchin.Abstracts;
using System.Collections.ObjectModel;

namespace Urchin
{
    class Encryptor : CryptoTransform
    {
        public Encryptor(byte[] key, byte[] iv, IKeySchedule keySchedule) : base(key, iv, keySchedule) {}

        public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] block = new byte[inputCount];
            Array.ConstrainedCopy(inputBuffer, inputOffset, block, 0, inputCount);
            // Firt round, mandatory Xor and Shuffle
            BitArray bits = new BitArray(block);
            int bitCount = bits.Count;
            ICollection<IWordTransformer> initialTransforms = InstatiateTransforms(InitialTransforms);
            foreach (IWordTransformer process in initialTransforms ) {
                process.WordSize = bitCount;
                process.Seed = keySchedule.GetNext(bitCount);
                bits = process.Encode(bits);
            }

            bits.CopyTo(block, 0);

            // Next 24-40 rounds
            BlockTransformer blockTransformer = new BlockTransformer(keySchedule);
            for (int i = 0; i < rounds; i++)
            {
                block = blockTransformer.Transform(block);
                blockTransformer.PseudoRandomize();
            }

            block.CopyTo(outputBuffer, outputOffset);

            return inputCount;
        }

        public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] outputBuffer = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            return outputBuffer;
        }

    }
}
