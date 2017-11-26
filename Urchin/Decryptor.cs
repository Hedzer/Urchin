using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Urchin.Interfaces;
using Urchin.Abstracts;

namespace Urchin
{
    class Decryptor : CryptoTransform
    {
        public Decryptor(byte[] key, byte[] iv, IKeySchedule keySchedule) : base(key, iv, keySchedule) { }

        public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] block = new byte[inputCount];
            Array.ConstrainedCopy(inputBuffer, inputOffset, block, 0, inputCount);

            // Decode rounds
            BlockEncoder blockEncoder = new BlockEncoder(keySchedule);
            block = blockEncoder.Decode(block, rounds);

            // Un-init block
            block = UninitializeBlock(block);
            block.CopyTo(outputBuffer, outputOffset);

            return inputCount;
        }

        public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] outputBuffer = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            return outputBuffer;
        }

        protected virtual byte[] UninitializeBlock(byte[] block)
        {
            return ApplyFirstBlockEncoding(block, Procedure.Decode);
        }
    }
}
