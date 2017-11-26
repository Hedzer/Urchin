using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Urchin.Interfaces;
using Urchin.Extensions.BitArray.ValueEquals;
using Urchin;


namespace UrchinTests.Encoders
{
    [TestClass, TestCategory("Encoders.Not")]
    public class Not
    {
        public static IWordEncoder instance = new Urchin.Encoders.Not();

        [TestMethod]
        public void Property_GetSet_WordSize()
        {
            for (int i = BlockEncoder.MinWordSize; i < BlockEncoder.MaxWordSize; i++)
            {
                instance.WordSize = i;
                Assert.AreEqual(instance.WordSize, i);
            }
        }

        [TestMethod]
        public void Property_Get_SeedSize()
        {
            Assert.AreEqual(instance.SeedSize, 0);
        }

        [TestMethod]
        public void Property_GetSet_Seed()
        {
            BitArray seed = new BitArray(instance.SeedSize);
            instance.Seed = seed;
            Assert.AreEqual(instance.Seed, seed);
        }

        [TestMethod]
        public void Property_GetSet_Entropy()
        {
            BitArray entropy = new BitArray(instance.SeedSize);
            instance.Entropy = entropy;
            Assert.AreEqual(instance.Entropy, entropy);
        }

        [TestMethod]
        public void Decode()
        {
            BitArray original = new BitArray(new bool[] { true, true, true, false, false, false });
            BitArray encoded = instance.Encode(original);
            BitArray decoded = instance.Decode(encoded);
            Assert.AreEqual(original.Length, decoded.Length);
            Assert.IsTrue(original.ValuesEqual(decoded));
            Assert.AreNotEqual(encoded, decoded);
        }

        [TestMethod]
        public void Encode()
        {
            BitArray original = new BitArray(new bool[] { true, true, true, false, false, false });
            BitArray encoded = instance.Encode(original);
            Assert.AreEqual(original.Length, encoded.Length);
            Assert.IsFalse(encoded.ValuesEqual(original));
        }
    }
}
