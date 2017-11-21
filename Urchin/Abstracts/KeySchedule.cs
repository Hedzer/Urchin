using System;
using System.Collections;
using Urchin.Interfaces;

namespace Urchin.Abstracts
{
    public abstract class KeySchedule : IKeySchedule
    {
        private byte[] key;
        private byte[] iv;
        public virtual byte[] Key
        {
            get
            {
                return key;
            }
            set
            {
                if (key != null) return;
                key = value;
            }
        }
        public virtual byte[] IV
        {
            get
            {
                return iv;
            }
            set
            {
                if (iv != null) return;
                iv = value;
            }
        }

        public abstract int CurrentStep { get; }
        public abstract BitArray GetNext(int bitCount);
        public abstract IKeySchedule CreateInstance();
        public abstract object Clone();

    }
}
