using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Urchin.Interfaces;
using Urchin.Transforms;
using System.Collections;

namespace Urchin
{
    class Encryptor : ICryptoTransform
    {

        // Urchin Encryptor
        private IKeySchedule keySchedule;
        private int rounds;
        public Encryptor(byte[] key, byte[] iv, IKeySchedule keySchedule)
        {
            IKeySchedule scheduler = keySchedule.CreateInstance();
            scheduler.Key = key;
            scheduler.IV = iv;
            this.keySchedule = scheduler;
            byte[] random = new byte[8];
            scheduler.GetNext(8).CopyTo(random, 0);
            rounds = random[0] % 16 + 24;
        }

        // ICryptoTransform
        public int InputBlockSize => 512;

        public int OutputBlockSize => 512;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => true;

        public void Dispose()
        {
            return;
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] block = new byte[inputCount];
            Array.ConstrainedCopy(inputBuffer, inputOffset, block, 0, inputCount);
            // Firt round, mandatory Xor and Shuffle
            BitArray bits = new BitArray(block);
            int bitCount = bits.Count;
            IWordTransformer[] initialTransforms = new IWordTransformer[] { (IWordTransformer)new Xor(), (IWordTransformer)new Shuffle() };
            foreach (IWordTransformer process in initialTransforms ) {
                process.WordSize = bitCount;
                process.Seed = keySchedule.GetNext(bitCount);
                bits = process.Transform(bits);
            }

            bits.CopyTo(block, 0);

            // Next 24-40 rounds
            BlockTransformer blockTransformer = new BlockTransformer(keySchedule);
            for (int i = 0; i < rounds; i++)
            {
                blockTransformer.Transform(block);
                blockTransformer.PseudoRandomize();
            }

            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }
    }
}
