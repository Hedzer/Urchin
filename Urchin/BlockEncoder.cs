using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;
using Urchin.Transforms;
using Urchin.Extensions.IEnumerable.Swap;
using Urchin.Extensions.BitArray.Words;

namespace Urchin
{
    class BlockEncoder : IBlockEncoder
    {
        private static Type[] possibleTransforms = new Type[]
        {
            typeof(Xor),
            typeof(Not),
            typeof(Reverse),
            typeof(Shuffle),
        };
        private IWordEncoder[] transforms;
        private int wordSize; // size of word in bits

        public IKeySchedule KeySchedule { get; set; }
        public IWordEncoder[] Transforms
        {
            get
            {
                return transforms;
            }
        }
        public Type[] PossibleTransforms
        {
            get
            {
                return possibleTransforms;
            }
        } 

        public BlockEncoder(IKeySchedule keySchedule)
        {
            KeySchedule = keySchedule ?? throw new ArgumentNullException();
            PseudoRandomize();
        }

        public byte[] DecodeBlock(byte[] block)
        {
            throw new NotImplementedException();
        }

        public byte[] EncodeBlock(byte[] block)
        {
            byte[] result = new byte[block.Length];
            int transformsCount = Transforms.Length;
            int bitsInBlock = block.Length * 8;
            BitArray bits = new BitArray(bitsInBlock);
            List<BitArray> words = new BitArray(block).ToWords(wordSize);
            int offset = 0;
            words.EachWord((BitArray word, int wordIndex) => {
                int wordLength = word.Length;
                IWordEncoder encoder = Transforms[wordIndex % transformsCount];
                encoder.WordSize = wordLength;
                encoder.Seed = KeySchedule.GetNext(encoder.SeedSize);
                BitArray encoded = encoder.Encode(word);
                encoded.EachBit((bool bit, int bitIndex) => {
                    bits[offset + bitIndex] = bit;
                    offset++;
                });
            });
            bits.CopyTo(result, 0);
            return result;
        }

        public byte[] Encode(byte[] block, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                block = EncodeBlock(block);
                PseudoRandomize();
            }
            return block;
        }

        public byte[] Decode(byte[] block, int iterations)
        {
            throw new NotImplementedException();
        }

        public void PseudoRandomize()
        {
            NewWordSize();
            InstantiateWordTransforms();
            ShuffleWordTransforms();
        }

        private void NewWordSize()
        {
            byte[] random = new byte[1];
            KeySchedule.GetNext(8).CopyTo(random, 0);
            wordSize = 24 + random[0] % 32;
        }

        private void InstantiateWordTransforms()
        {
            int count = PossibleTransforms.Length;
            for (int i = 0; i < count; i++)
            {
                IWordEncoder transform = (IWordEncoder)Activator.CreateInstance(PossibleTransforms[i]);
                transform.WordSize = wordSize;
                if (transform.SeedSize != 0)
                {
                    transform.Seed = KeySchedule.GetNext(transform.SeedSize);
                }

                transforms[i] = transform;
            }
        }

        private void ShuffleWordTransforms()
        {
            int count = transforms.Length;
            byte[] random = new byte[count];
            KeySchedule.GetNext(count * 8).CopyTo(random, 0);
            for (int i = 0; i < count; i++)
            {
                int r = random[i] % count;
                transforms.Swap(i, r);
            }
        }
    }
}
