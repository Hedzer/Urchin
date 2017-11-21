using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Urchin.Interfaces;

namespace Urchin
{
    public class Cipher : SymmetricAlgorithm, ICipher
    {
        // Random Key Generator, SHA512 of a random number
        public virtual byte[] RKG()
        {
            HMACSHA512 hasher = new HMACSHA512();
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] random = new byte[64];
                rng.GetBytes(random);
                return hasher.ComputeHash(random);
            }
        }

        // SymmetricAlgorithm
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return CreateEncryptor(rgbKey, rgbIV, keySchedule);
        }

        public ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV, IKeySchedule keySchedule)
        {
            return new Encryptor(rgbKey, rgbIV, keySchedule);
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return CreateDecryptor(rgbKey, rgbIV, keySchedule);
        }

        public ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV, IKeySchedule keySchedule)
        {
            return new Decryptor(rgbKey, rgbIV, keySchedule);
        }

        public override byte[] Key {
            get
            {
                return KeySchedule.Key;
            }
            set
            {
                KeySchedule.Key = value;

            }
        }

        public override byte[] IV
        {
            get
            {
                return KeySchedule.IV;
            }
            set
            {
                KeySchedule.IV = value;

            }
        }

        // IUrchin
        private IKeySchedule keySchedule = new KeyScheduler();

        public IKeySchedule KeySchedule {
            get
            {
                return keySchedule;
            }
            set
            {
                keySchedule = value;
            }
        }

        public override void GenerateKey()
        {
            Key = RKG();
        }

        public override void GenerateIV()
        {
            IV = RKG();
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            throw new NotImplementedException();
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            throw new NotImplementedException();
        }

        public byte[] Encrypt(string plaintext)
        {
            throw new NotImplementedException();
        }

        public byte[] Decrypt(string ciphertext)
        {
            throw new NotImplementedException();
        }
    }
}
