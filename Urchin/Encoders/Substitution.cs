using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Urchin.Extensions.BitArray.Words;
using Urchin.Extensions.IEnumerable.Swap;
using Urchin.Interfaces;

namespace Urchin.Encoders
{
    public class Substitution : IWordEncoder
    {
        // Minimum word size in bits for thr table
        private static byte[] tableWordSizes = new byte[] { 8, 7, 6, 5, 4, 3, 2 };
        private static HashAlgorithm hasher = new RIPEMD160Managed();
        private int wordSize = 0;
        private int seedSize = 512;
        private int tableSize = 5;
        private BitArray seed;

        public int WordSize
        {
            get => wordSize;
            set => wordSize = value;
        }

        public int SeedSize => seedSize;

        public BitArray Seed
        {
            get => seed;
            set => seed = value;
        }
        public BitArray Entropy { get; set; }

        public BitArray Decode(BitArray word)
        {
            return Encode(word);
        }

        public BitArray Encode(BitArray word)
        {

            Shuffle shuffler = GetShuffler(wordSize, seed);
            BitArray result = new BitArray(word);
            byte tableSize = GetTableSize(wordSize, seed);
            byte[] table = BuildTable(tableSize, seed);
            List<BitArray> words = result.ToWords(tableSize);

            return result;
        }

        private byte GetTableSize(int wordSize, BitArray seed)
        {
            foreach(byte number in tableWordSizes)
            {
                if (wordSize % number == 0) return number;
            }
            byte[] seedAsBytes = new byte[64];
            seed.CopyTo(seedAsBytes, 0);
            return tableWordSizes[seedAsBytes[seedAsBytes[0] % 64] % tableWordSizes.Length];
        }

        private byte[] BuildTable(int tableWordSize, BitArray seed, bool inverse = false)
        {
            int maxTableValue = (int)Math.Pow(2, tableWordSize);
            byte[] seedAsBytes = new byte[64];
            seed.CopyTo(seedAsBytes, 0);
            List<byte> entropy = new List<byte>(hasher.ComputeHash(seedAsBytes));
            do
            {
                entropy.AddRange(hasher.ComputeHash(entropy.ToArray()));
            } while (entropy.Count < maxTableValue);

            byte[] result = new byte[maxTableValue];
            for (byte i = 0; i < maxTableValue; i++) result[i] = i;
            for (byte j = 0; j < maxTableValue; j++) result.Swap(j, entropy[j]);
            return result;
        }

        private Shuffle GetShuffler(int wordSize, BitArray seed)
        {
            Shuffle shuffler = new Shuffle { WordSize = wordSize };
            return shuffler;
        }
    }
}
