using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Urchin.Encoders;
using Urchin.Interfaces;

namespace Urchin.Abstracts
{
    public abstract class CryptoTransform : ICryptoTransform
    {
        protected IKeySchedule keySchedule;
        protected int rounds;
        public static readonly int MinRounds = 24;
        public static readonly int MaxAdditionalRounds = 16;
        protected enum Procedure
        {
            Encode,
            Decode,
        }
        public CryptoTransform(byte[] key, byte[] iv, IKeySchedule keySchedule)
        {
            IKeySchedule scheduler = (IKeySchedule)Activator.CreateInstance(keySchedule.GetType());
            scheduler.Key = key;
            scheduler.IV = iv;
            this.keySchedule = scheduler;
            byte[] random = new byte[8];
            scheduler.GetNext(8).CopyTo(random, 0);
            rounds = MinRounds + (random[0] % (MaxAdditionalRounds + 1));
        }

        public static ReadOnlyCollection<Type> InitialTransforms { get; } = new ReadOnlyCollection<Type>(new List<Type> { typeof(Xor), typeof(Shuffle) });

        public virtual int InputBlockSize => 512;

        public virtual int OutputBlockSize => 512;

        public virtual bool CanTransformMultipleBlocks => true;

        public virtual bool CanReuseTransform => true;

        public virtual void Dispose()
        {
            return;
        }

        public abstract int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

        public abstract byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);

        protected virtual List<IWordEncoder> CreateInitialBlockPlan(int blockByteCount)
        {
            List<IWordEncoder> result = new List<IWordEncoder>() { };
            int bitCount = blockByteCount * 8;
            ICollection<IWordEncoder> initialTransforms = InstatiateTransforms(InitialTransforms);
            foreach (IWordEncoder process in initialTransforms)
            {
                process.WordSize = bitCount;
                process.Seed = keySchedule.GetNext(bitCount);
                result.Add(process);
            }
            return result;
        }

        protected List<IWordEncoder> InstatiateTransforms(ICollection<Type> transforms)
        {
            List<IWordEncoder> result = new List<IWordEncoder> { };
            foreach (Type item in transforms)
            {
                result.Add((IWordEncoder)Activator.CreateInstance(item));
            }
            return result;
        }
    }
}
