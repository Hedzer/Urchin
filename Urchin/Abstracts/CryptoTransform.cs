using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;
using System.Security.Cryptography;
using Urchin.Transforms;
using System.Collections.ObjectModel;

namespace Urchin.Abstracts
{
    abstract class CryptoTransform : ICryptoTransform
    {
        protected IKeySchedule keySchedule;
        protected int rounds;
        public CryptoTransform(byte[] key, byte[] iv, IKeySchedule keySchedule)
        {
            IKeySchedule scheduler = keySchedule.CreateInstance();
            scheduler.Key = key;
            scheduler.IV = iv;
            this.keySchedule = scheduler;
            byte[] random = new byte[8];
            scheduler.GetNext(8).CopyTo(random, 0);
            rounds = random[0] % 16 + 24;
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

        protected List<IWordTransformer> InstatiateTransforms(ICollection<Type> transforms)
        {
            List<IWordTransformer> result = new List<IWordTransformer> { };
            foreach (Type item in transforms)
            {
                result.Add((IWordTransformer)Activator.CreateInstance(item));
            }
            return result;
        }
    }
}
