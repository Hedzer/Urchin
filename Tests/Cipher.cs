using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;




namespace Tests
{
    [TestClass]
    public class Cipher
    {
        public static void Main()
        {
            
        }
        [TestMethod][TestCategory("Cipher")]
        public void EncryptDecrypt()
        {
            HashAlgorithm hasher = new SHA512Managed();
            Urchin.Cipher cipher = new Urchin.Cipher();
            cipher.Key = hasher.ComputeHash(new byte[] { 1 });
            cipher.IV = hasher.ComputeHash(cipher.Key);
            for (byte i = 0; i < 100; i++)
            {
                byte[] original = hasher.ComputeHash(new byte[] { i });
                byte[] encrypted = cipher.Encrypt(original);
                byte[] decrypted = cipher.Decrypt(encrypted);
                CollectionAssert.AreEqual(original, decrypted);
            }
        }
    }
}
