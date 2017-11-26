using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Types;
using Urchin.Interfaces;
using Urchin.Encoders;
using Urchin.Extensions.IEnumerable.Swap;
using Urchin.Extensions.BitArray.Words;

namespace Urchin
{
    public class BlockEncoder : IBlockEncoder
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

        public static readonly int MinWordSize = 16;
        public static readonly int MaxWordSize = 64;
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

        public EncodingRound GetRoundSnapshot(int blockLength)
        {
            EncodingRound result = new EncodingRound();
            result.WordSize = wordSize;
            int transformsCount = Transforms.Length;
            int bitsInBlock = blockLength * 8;
            int remainder = bitsInBlock % wordSize;
            bool hasRemainder = remainder == 0;
            int wordCount = (int)Math.Ceiling((double)bitsInBlock / wordSize);
            for (int index = 0; index < wordCount; index++)
            {
                int wordLength = (index == wordCount - 1  && hasRemainder ? remainder : wordSize);
                EncoderProxy encoder = new EncoderProxy
                {
                    WordEncoder = transforms[index % transformsCount].GetType(),
                    WordSize = wordLength,
                };
                encoder.Seed = KeySchedule.GetNext(encoder.SeedSize);
                result.Transformations.Add(encoder);
            }
            return result;
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
            byte[] result = block;
            for (int i = 0; i < iterations; i++)
            {
                result = EncodeBlock(result);
                PseudoRandomize();
            }
            return result;
        }

        public byte[] DecodeBlock(byte[] block, EncodingRound snapshot)
        {
            byte[] result = new byte[block.Length];
            List<BitArray> buffer = new List<BitArray> { };
            List<BitArray> words = new BitArray(block).ToWords(snapshot.WordSize);
            for (int i = words.Count; i > 0; i--)
            {
                IWordEncoder decoder = snapshot.Transformations[i];
                BitArray decoded = decoder.Decode(words[i]);
                buffer.Insert(0, decoded);
            }
            buffer.ToBitArray().CopyTo(result, 0);
            return result;
        }

        public byte[] Decode(byte[] block, int iterations)
        {
            byte[] result = block;
            List<EncodingRound> rounds = new List<EncodingRound> { };
            for (int i = 0; i < iterations; i++)
            {
                rounds.Add(GetRoundSnapshot(block.Length));
                PseudoRandomize();
            }
            rounds.Reverse();
            rounds.ForEach((EncodingRound round) => {
                result = DecodeBlock(result, round);
            });

            return result;
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
            wordSize = (MinWordSize + random[0]) % MaxWordSize;
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
