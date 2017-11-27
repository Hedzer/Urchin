using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Urchin.Extensions.BitArray.Words;

namespace Tests.Extensions
{
    [TestClass, TestCategory("Extensions.BitArray.Words")]
    public class Words
    {
        [TestMethod]
        public void BitArrayToWords()
        {
            HashAlgorithm hasher = new SHA512Managed();
            BitArray source = new BitArray(hasher.ComputeHash(new byte[] { 1, 2, 3, 4, 5 }));
            List<BitArray> words = source.ToWords(8);
            Assert.AreEqual(source.Length / 8, words.Count);
        }
    }
}
