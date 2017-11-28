using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Urchin.Extensions.BitArray.Words;

namespace Tests.Extensions.BitArray
{
    [TestClass, TestCategory("Extensions.BitArray.Words")]
    public class Words
    {
        [TestMethod]
        public void BitArrayToWords()
        {
            HashAlgorithm hasher = new SHA512Managed();
            System.Collections.BitArray source = new System.Collections.BitArray(hasher.ComputeHash(new byte[] { 1, 2, 3, 4, 5 }));
            List<System.Collections.BitArray> words = source.ToWords(8);
            Assert.AreEqual(64, words.Count);
        }
        [TestMethod]
        public void ListOfBitArrayToBitArray()
        {
            HashAlgorithm hasher = new SHA512Managed();
            byte[] hash = hasher.ComputeHash(new byte[] { 1, 2, 3, 4, 5 });
            for (int i = 3; i < 256; i++)
            {
                hash = hasher.ComputeHash(hash);
                System.Collections.BitArray original = new System.Collections.BitArray(hash);
                List<System.Collections.BitArray> words = original.ToWords(i);
                System.Collections.BitArray reconstituted = words.ToBitArray();
                CollectionAssert.AreEqual(original, reconstituted);
            }
        }

    }
}
