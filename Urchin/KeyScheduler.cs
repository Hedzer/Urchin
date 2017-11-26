using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Abstracts;
using Urchin.Interfaces;
using Urchin.Extensions.IEnumerable.Swap;
using System.Security.Cryptography;

namespace Urchin
{
    public class KeyScheduler : KeySchedule
    {
        private int currentStep = 0;
        private List<byte> buffer = new List<byte> { };
        private byte[] state = new byte[64];
        private static HashAlgorithm[] algorithms = new HashAlgorithm[]
        {
            new MD5Cng(),
            new SHA1Managed(),
            new SHA256Managed(),
            new SHA384Managed(),
            new SHA512Managed(),
            new RIPEMD160Managed(),
        };
        private HashAlgorithm[] hashes = (HashAlgorithm[])algorithms.Clone();

        public override int CurrentStep => currentStep;

        public override BitArray GetNext(int bitCount)
        {
            buffer.Clear();
            BitArray result = new BitArray(bitCount);
            int byteCount = (int)Math.Ceiling((double)bitCount / 8);
            do
            {
                HashAlgorithm hasher = hashes[currentStep % hashes.Length];
                List<byte> entropy = new List<byte> { };
                byte[][] sources = new byte[][] { BitConverter.GetBytes(currentStep), Secret, IV, Key, state };
                foreach (byte[] item in sources) entropy.AddRange(item);
                byte[] hash = hasher.ComputeHash(entropy.ToArray());
                buffer.AddRange(hash);
                int hashLength = hash.Length;
                int a = hash[currentStep % hashLength];
                int b = hash[a % hashLength];
                hashes.Swap(a, b);
                new BitArray(state).Xor(new BitArray(hash)).CopyTo(state, 0);
                currentStep++;
            } while (byteCount > buffer.Count);

            BitArray pool = new BitArray(buffer.ToArray());
            for (int i = 0; i < bitCount; i++) result[i] = pool[i];
            return result;
        }

        protected override bool CreateSecret()
        {
            bool wasCreated = base.CreateSecret();
            if (!wasCreated) return false;
            MixHashes();
            return true;
        }

        public override object Clone()
        {
            KeyScheduler clone = (KeyScheduler)Activator.CreateInstance(GetType());
            clone.Key = Key;
            clone.IV = IV;
            clone.currentStep = currentStep;
            return clone;
        }

        private void MixHashes()
        {
            int secretLength = Secret.Length;
            int hashesLength = hashes.Length;
            for (int i = 0; i < secretLength; i++)
            {
                hashes.Swap(i % hashesLength, Secret[i] % hashesLength);
            }
        }
    }
}
