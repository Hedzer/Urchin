using System.Collections;
using System.Security.Cryptography;
using Urchin.Interfaces;

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
        public abstract object Clone();

        protected virtual bool CreateSecret()
        {
            const int MIN_LENGTH = 128;
            const int MAX_OFFSET = 64;
            if (key == null || iv == null) return false;
            SHA512Managed hasher = new SHA512Managed();
            byte[] combined = new byte[64];
            new BitArray(key).Xor(new BitArray(iv)).CopyTo(combined, 0);
            secret = hasher.ComputeHash(combined);
            int secretIndex = combined[0] % secret.Length;
            int secretLength = secret[secretIndex];
            int combinedLength = combined[secretLength % combined.Length];
            int length = MIN_LENGTH + (combinedLength % MAX_OFFSET) + (secretLength % MAX_OFFSET);
            for (int i = 0; i < length; i++) secret = hasher.ComputeHash(secret);
            return true;
        }

    }
}
