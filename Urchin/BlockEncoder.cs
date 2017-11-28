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

        public EncodingPlan GetEncodingPlan(int blockLength)
        {
            EncodingPlan result = new EncodingPlan();
            result.WordSize = wordSize;
            int transformsCount = Transforms.Length;
            int bitsInBlock = blockLength * 8;
            int remainder = bitsInBlock % wordSize;
            bool hasRemainder = remainder != 0;
            int wordCount = (int)Math.Ceiling((double)bitsInBlock / wordSize);
            for (int index = 0; index < wordCount; index++)
            {
                int wordLength = (index == wordCount - 1  && hasRemainder ? remainder : wordSize);
                IWordEncoder encoder = (IWordEncoder)Activator.CreateInstance(transforms[index % transformsCount].GetType());
                encoder.WordSize = wordLength;
                encoder.Seed = KeySchedule.GetNext(encoder.SeedSize);
                result.Transformations.Add(encoder);
            }
            return result;
        }

        public List<EncodingPlan> GetEncodingPlans(int blockLength, int iterations)
        {
            List<EncodingPlan> plans = new List<EncodingPlan> { };
            for (int i = 0; i < iterations; i++)
            {
                plans.Add(GetEncodingPlan(blockLength));
                PseudoRandomize();
            }
            return plans;
        }

        public byte[] EncodeBlock(EncodingPlan plan, byte[] block)
        {
            byte[] result = new byte[block.Length];
            List<BitArray> words = new BitArray(block).ToWords(plan.WordSize);
            int wordCount = words.Count;
            for (int i = 0; i < wordCount; i++)
            {
                IWordEncoder encoder = plan.Transformations[i];
                BitArray encoded = encoder.Encode(words[i]);
                words[i] = encoded;
            }
            words.ToBitArray().CopyTo(result, 0);
            return result;
        }

        public byte[] Encode(byte[] block, int iterations)
        {
            byte[] result = block;
            GetEncodingPlans(block.Length, iterations).ForEach((EncodingPlan plan) => {
                result = EncodeBlock(plan, result);
            });
            return result;
        }

        public byte[] DecodeBlock(EncodingPlan plan, byte[] block)
        {
            byte[] result = new byte[block.Length];
            List<BitArray> buffer = new List<BitArray> { };
            List<BitArray> words = new BitArray(block).ToWords(plan.WordSize);
            for (int i = words.Count - 1; i >= 0; i--)
            {
                IWordEncoder decoder = plan.Transformations[i];
                BitArray decoded = decoder.Decode(words[i]);
                buffer.Insert(0, decoded);
            }
            buffer.ToBitArray().CopyTo(result, 0);
            return result;
        }

        public byte[] Decode(byte[] block, int iterations)
        {
            byte[] result = block;
            List<EncodingPlan> plans = GetEncodingPlans(block.Length, iterations);
            plans.Reverse();
            plans.ForEach((EncodingPlan plan) => {
                result = DecodeBlock(plan, result);
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
            wordSize = (MinWordSize + random[0] % (MaxWordSize - MinWordSize + 1));
        }

        private void InstantiateWordTransforms()
        {
            int count = PossibleTransforms.Length;
            transforms = new IWordEncoder[count];
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
