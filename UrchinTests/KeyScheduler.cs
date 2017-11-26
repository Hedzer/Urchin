using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Collections;
using Urchin;
using Urchin.Extensions.BitArray.ValueEquals;

namespace UrchinTests
{
    [TestClass][TestCategory("KeyScheduler")]
    public class KeySchedulerTest
    {
        private byte[] iv;
        private byte[] key;
        private KeyScheduler instance = new KeyScheduler();
        public KeySchedulerTest()
        {
            HashAlgorithm hasher = new SHA512Managed();
            iv = hasher.ComputeHash(new byte[] { 1, 2, 3, 4, 5 });
            key = hasher.ComputeHash(iv);
        }



        [TestMethod]
        public void GetSetKey()
        {
            instance.Key = key;
            byte[] duplicate = new byte[key.Length];
            key.CopyTo(duplicate, 0);
            Assert.AreEqual(instance.Key, duplicate);
        }

        [TestMethod]
        public void GetSetIV()
        {
            instance.IV = iv;
            byte[] duplicate = new byte[iv.Length];
            key.CopyTo(duplicate, 0);
            Assert.AreEqual(instance.IV, duplicate);
        }

        [TestMethod]
        public void GetSecret()
        {
            Assert.AreEqual(instance.Secret.GetType(), (new byte[0]).GetType());
        }

        [TestMethod]
        public void IsPseudoRandom()
        {
            KeyScheduler a = new KeyScheduler
            {
                Key = key,
                IV = iv,
            };
            KeyScheduler b = new KeyScheduler
            {
                Key = key,
                IV = iv,
            };

            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(a.GetNext(i).ValuesEqual(b.GetNext(i)));
            }
        }

        [TestMethod]
        public void GetNext()
        {
            KeyScheduler a = new KeyScheduler
            {
                Key = key,
                IV = iv
            };

            for (int i = 32; i <= 100; i++)
            {
                BitArray current = a.GetNext(i);
                Assert.IsFalse(a.GetNext(i).ValuesEqual(current));
                Assert.AreEqual(current.Length, i);
            }
        }

        [TestMethod]
        public void Clone()
        {
            KeyScheduler a = new KeyScheduler
            {
                Key = key,
                IV = iv,
            };

            for (int i = 0; i < 10; i++)
            {
                a.GetNext(i);
            }

            KeyScheduler b = (KeyScheduler)a.Clone();
            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(a.GetNext(i).ValuesEqual(b.GetNext(i)));
            }
        }
    }
}
