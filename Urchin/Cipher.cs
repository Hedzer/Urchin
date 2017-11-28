using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Urchin.Interfaces;

namespace Urchin
{
    public class Cipher : SymmetricAlgorithm, ICipher
    {
        // Random Key Generator, SHA512 of a random number
        public virtual byte[] RandomKeyGenerator()
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
            Key = RandomKeyGenerator();
        }

        public override void GenerateIV()
        {
            IV = RandomKeyGenerator();
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] result;
            MemoryStream memory = new MemoryStream();
            ICryptoTransform encryptor = CreateEncryptor();
            CryptoStream stream = new CryptoStream(memory, encryptor, CryptoStreamMode.Write);
            stream.Write(plaintext, 0, plaintext.Length);
            stream.FlushFinalBlock();
            result = memory.ToArray();
            memory.Close();
            stream.Close();
            return result;
        }

        public byte[] Encrypt(string plaintext)
        {
            return Encrypt(Encoding.ASCII.GetBytes(plaintext));
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            byte[] result;
            MemoryStream memory = new MemoryStream();
            ICryptoTransform decryptor = CreateDecryptor();
            CryptoStream stream = new CryptoStream(memory, decryptor, CryptoStreamMode.Write);
            stream.Write(ciphertext, 0, ciphertext.Length);
            stream.FlushFinalBlock();
            result = memory.ToArray();
            memory.Close();
            stream.Close();
            return result;
        }

        public byte[] Decrypt(string ciphertext)
        {
            return Decrypt(ciphertext);
        }
    }
}
