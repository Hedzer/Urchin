using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Urchin {
	public static class Processes {
		public delegate BitArray WordProcessDelegate(string seed, BitArray data, bool inverse);
		public static WordProcessDelegate[] UnaryProcesses = {
			WordUnaryNot,
			WordUnaryReverse
		};
		public static WordProcessDelegate[] BinaryProcesses = {
			WordBinaryXor,
			WordBinaryShuffle
		};
		private static readonly MD5 hasher = MD5.Create();
		private static BitArray GetBitEntropy(string seed, int size){
			byte[] hash =  GetByteEntropy(seed, size/8+1);
			BitArray entropy = new BitArray(hash);
			entropy.Length = size;
			return entropy;
		}
		private static byte[] GetByteEntropy(string seed, int size){
			byte[] hash =  hasher.ComputeHash(System.Text.Encoding.ASCII.GetBytes(seed));
			List<byte> entropy = new List<byte>();
			entropy.AddRange(hash);
			int rounds = size / 32;
			byte[] lastHash = hash;
			for (int i = 0; i < rounds; i++){
				byte[] currentHash = hasher.ComputeHash(lastHash);
				entropy.AddRange(currentHash);
				lastHash = currentHash;
			}
			entropy.RemoveRange(size, entropy.Count - size);
			return entropy.ToArray();
		}
		private static void ReverseBitArray(BitArray array){
		    int length = array.Length;
		    int mid = (length / 2);
		    for (int i = 0; i < mid; i++) {
		        bool bit = array[i];
		        array[i] = array[length - i - 1];
		        array[length - i - 1] = bit;
		    }
		}
		public static BitArray WordUnaryNot(string seed, BitArray data, bool inverse){
			BitArray result = new BitArray(data);
			result.Not();
			return result;
		}
		public static BitArray WordUnaryReverse(string seed, BitArray data, bool inverse){
			BitArray result = new BitArray(data);
			ReverseBitArray(result);
			return result;
		}		
		public static BitArray WordBinaryXor(string seed, BitArray data, bool inverse){
			BitArray result = new BitArray(data);
			result.Xor(GetBitEntropy(seed, result.Length));
			return result;
		}
		public static BitArray WordBinaryShuffle(string seed, BitArray data, bool inverse){
			BitArray result = new BitArray(data);
			byte[] entropy = GetByteEntropy(seed, result.Length);
			//this might take a bit more work
			if (inverse){
				
			}
			foreach(int i in result){
				
			}
			return result;
		}
	}
}
