using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Urchin;
using Urchin.Interfaces;

namespace Tests.Encoders
{
    [TestClass, TestCategory("Encoders")]
    public class Generic<WordEncoder> where WordEncoder : IWordEncoder, new()
    {
        public IWordEncoder instance = new WordEncoder();

        public Generic() {
            instance = GetNewInstance(BlockEncoder.MaxWordSize);
        }

        [TestMethod]
        public virtual void Property_GetSet_WordSize()
        {
            for (int i = BlockEncoder.MinWordSize; i < BlockEncoder.MaxWordSize; i++)
            {
                instance.WordSize = i;
                Assert.AreEqual(instance.WordSize, i);
            }
        }

        [TestMethod]
        public virtual void Property_Get_SeedSize()
        {
            Assert.IsTrue(instance.SeedSize >= 0);
        }

        [TestMethod]
        public virtual void Property_GetSet_Seed()
        {
            BitArray seed = new BitArray(instance.SeedSize);
            instance.Seed = seed;
            CollectionAssert.AreEqual(instance.Seed, seed);
        }

        [TestMethod]
        public virtual void Property_GetSet_Entropy()
        {
            BitArray entropy = new BitArray(instance.SeedSize);
            instance.Entropy = entropy;
            CollectionAssert.AreEqual(instance.Entropy, entropy);
        }

        [TestMethod]
        public virtual void Decode()
        {
            BitArray original = GetWord();
            BitArray encoded = instance.Encode(original);
            BitArray decoded = instance.Decode(encoded);
            Assert.AreEqual(original.Length, decoded.Length);
            CollectionAssert.AreEqual(original, decoded);
            Assert.AreNotEqual(encoded, decoded);
        }

        [TestMethod]
        public virtual void Encode()
        {
            BitArray original = GetWord();
            BitArray encoded = instance.Encode(original);
            Assert.AreEqual(original.Length, encoded.Length);
            CollectionAssert.AreNotEqual(encoded, original);
        }

        [TestMethod]
        public virtual void LastWordEncode()
        {
            BitArray original = GetWord(8);
            IWordEncoder encoder = GetNewInstance();
            BitArray encoded = encoder.Encode(original);
            Assert.AreEqual(original.Length, encoded.Length);
            CollectionAssert.AreNotEqual(encoded, original);
        }

        [TestMethod]
        public virtual void LastWordDecode()
        {
            BitArray original = GetWord(8);
            IWordEncoder decoder = GetNewInstance();
            BitArray encoded = decoder.Encode(original);
            BitArray decoded = decoder.Decode(encoded);
            Assert.AreEqual(original.Length, decoded.Length);
            CollectionAssert.AreEqual(original, decoded);
            Assert.AreNotEqual(encoded, decoded);
        }

        private WordEncoder GetNewInstance()
        {
            return GetNewInstance(BlockEncoder.MaxWordSize);
        }
        private WordEncoder GetNewInstance(int wordSize)
        {
            WordEncoder encoder = new WordEncoder();
            HashAlgorithm hasher = new SHA512Managed();
            byte[] key = hasher.ComputeHash(new byte[] { 1, 2, 3 });
            byte[] iv = hasher.ComputeHash(key);

            KeyScheduler keyScheduler = new KeyScheduler
            {
                Key = key,
                IV = iv,
            };
            encoder.WordSize = wordSize;
            encoder.Seed = keyScheduler.GetNext(encoder.SeedSize);
            encoder.Entropy = new BitArray(hasher.ComputeHash(iv));
            return encoder;
        }
        private BitArray GetWord()
        {
            return GetWord(BlockEncoder.MaxWordSize);
        }
        private BitArray GetWord(int wordSize)
        {
            List<bool> list = new List<bool>() { };
            for (int i = 0; i < wordSize; i++) list.Add(i % 2 == 1);
            return new BitArray(list.ToArray());
        }
    }
}
