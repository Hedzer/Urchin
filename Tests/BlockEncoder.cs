using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Urchin;
using Urchin.Interfaces;
using Urchin.Types;
using Urchin.Encoders;
using Urchin.Abstracts;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tests
{
    [TestClass][TestCategory("BlockEncoder")]
    public class BlockEncoderTest
    {
        private static HashAlgorithm hasher = new SHA512Managed();
        private static byte[] Key = hasher.ComputeHash(new byte[] { 1, 2, 3, 4, 5 });
        private static byte[] IV = hasher.ComputeHash(new byte[] { 5, 4, 3, 2, 1 });
        private static KeyScheduler keyScheduler = new KeyScheduler { Key = Key, IV = IV };
        private static BlockEncoder instance = new BlockEncoder(keyScheduler);

        [TestMethod]
        public void Field_MinWordSize()
        {
            Assert.AreEqual(BlockEncoder.MinWordSize.GetType(), 1.GetType());
            Assert.IsTrue(BlockEncoder.MinWordSize < BlockEncoder.MaxWordSize);
            FieldInfo info = instance.GetType().GetField("MinWordSize");
            Assert.IsTrue(info.IsInitOnly);
            Assert.IsTrue(info.IsStatic);
        }

        [TestMethod]
        public void Field_MaxWordSize()
        {
            Assert.AreEqual(BlockEncoder.MaxWordSize.GetType(), 1.GetType());
            Assert.IsTrue(BlockEncoder.MinWordSize < BlockEncoder.MaxWordSize);
            FieldInfo info = instance.GetType().GetField("MaxWordSize");
            Assert.IsTrue(info.IsInitOnly);
            Assert.IsTrue(info.IsStatic);
        }

        [TestMethod]
        public void Property_GetSet_KeySchedule()
        {
            Assert.AreEqual(instance.KeySchedule, keyScheduler);
        }

        [TestMethod]
        public void Property_Get_Transforms()
        {
            Assert.IsTrue(instance.Transforms.Length > 0);
            foreach (var item in instance.Transforms) Assert.IsTrue(item is IWordEncoder);
        }

        [TestMethod]
        public void Propert_Get_PossibleTransforms()
        {
            Assert.IsTrue(instance.PossibleTransforms.Length > 0);
            foreach (var item in instance.Transforms) Assert.IsTrue(typeof(IWordEncoder).IsAssignableFrom(item.GetType()));
        }

        [TestMethod]
        public void GetRoundSnapshot()
        {
            for (int i = 0; i < 25; i++)
            {
                EncodingRound result = instance.GetRoundSnapshot(blockLength: 64) as EncodingRound;
                Assert.IsNotNull(result);
                Assert.IsTrue(result.WordSize >= BlockEncoder.MinWordSize && result.WordSize <= BlockEncoder.MaxWordSize);
                List<EncoderProxy> transformations = result.Transformations;
                Assert.IsTrue(transformations.Count > 0);
            }
        }

        [TestMethod]
        public void EncodeBlock()
        {
            BlockEncoder instance = new BlockEncoder(new KeyScheduler { Key = Key, IV = IV });
            byte[] block = hasher.ComputeHash(Key);
            byte[] encoded = instance.EncodeBlock(block);
            CollectionAssert.AreNotEqual(encoded, block);
        }

        [TestMethod]
        public void Encode()
        {
            List<string> results = new List<string>() { };
            for (int i = CryptoTransform.MinRounds; i < CryptoTransform.MinRounds + CryptoTransform.MaxAdditionalRounds; i++)
            {
                BlockEncoder instance = new BlockEncoder(new KeyScheduler { Key = Key, IV = IV });
                byte[] block = hasher.ComputeHash(Key);
                byte[] encoded = instance.Encode(block, i);
                Assert.IsTrue(block.Length == encoded.Length);
                results.Add(Encoding.ASCII.GetString(encoded));
            }
            CollectionAssert.AllItemsAreUnique(results);
        }

        [TestMethod]
        public void DecodeBlock()
        {

        }

        [TestMethod]
        public void Decode()
        {
            for (int i = CryptoTransform.MinRounds; i < CryptoTransform.MinRounds + CryptoTransform.MaxAdditionalRounds; i++)
            {
                BlockEncoder encoder = new BlockEncoder(new KeyScheduler { Key = Key, IV = IV });
                BlockEncoder decoder = new BlockEncoder(new KeyScheduler { Key = Key, IV = IV });
                byte[] block = hasher.ComputeHash(Key);
                byte[] encoded = encoder.Encode(block, i);
                byte[] decoded = decoder.Decode(encoded, i);
                Assert.IsTrue(block.Length == decoded.Length);
                CollectionAssert.AreEqual(decoded, block);
            }
        }
    }
}
