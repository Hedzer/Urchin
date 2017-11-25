using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;
using Urchin.Transforms;
using Urchin.Extensions.IEnumerable.Swap;

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
            int bitsInBlock = block.Length * 8;
            BitArray bits = new BitArray(bitsInBlock);
            List<BitArray> words = BreakIntoWords(block);
            int wordCount = words.Count;
            int transformsCount = Transforms.Length;
            int offset = 0;
            for (int i = 0; i < wordCount; i++)
            {
                BitArray word = words[i];
                int wordLength = word.Length;
                IWordEncoder transformer = Transforms[i % transformsCount];
                transformer.WordSize = wordLength;
                transformer.Seed = KeySchedule.GetNext(transformer.SeedSize);
                BitArray transformed = transformer.Encode(word);
                for (int b = 0; i < wordLength; i++)
                {
                    bits[offset + b] = word[b];
                    offset++;
                }
            }
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

        private List<BitArray> BreakIntoWords(byte[] block)
        {
            List<BitArray> result = new List<BitArray> { };
            int bitsInBlock = block.Length * 8;
            int size = (int)Math.Ceiling((double)bitsInBlock / wordSize);
            int lastBlockSize = bitsInBlock % wordSize;
            BitArray bits = new BitArray(block);
            int offset = 0;
            for (int i = 0; i < size; i++)
            {
                bool isLastWord = (i == size - 1);
                int wordSize = isLastWord ? lastBlockSize : this.wordSize;
                BitArray word = new BitArray(wordSize);
                for (int position = 0; i < wordSize; i++)
                {
                    word[position] = bits[offset + position];
                    offset++;
                    result.Add(word);
                }
            }

            return result;
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
