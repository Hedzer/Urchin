using System;
using System.Collections;
using Urchin.Interfaces;
using System.Security.Cryptography;

namespace Urchin.Abstracts
{
    public abstract class KeySchedule : IKeySchedule
    {
        private byte[] key;
        private byte[] iv;
        private byte[] secret;
        public virtual byte[] Secret
        {
            get
            {
                return secret;
            }
        }
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
                CreateSecret();
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
                CreateSecret();
            }
        }

        public abstract int CurrentStep { get; }
        public abstract BitArray GetNext(int bitCount);
        public abstract IKeySchedule CreateInstance();
        public abstract object Clone();

        protected virtual bool CreateSecret()
        {
            if (key == null || iv == null) return false;
            byte[] combined = new byte[64];
            SHA512Managed hasher = new SHA512Managed();
            new BitArray(key).Xor(new BitArray(iv)).CopyTo(combined, 0);
            secret = hasher.ComputeHash(combined);
            return true;
        }

    }
}
