using System;
using System.Collections;

namespace Urchin {
	public static class Processes {
		public delegate BitArray ProcessDelegate(string seed, BitArray data, bool inverse);
		public static BitArray UnaryNot(string seed, BitArray data, bool inverse){
			BitArray result = new BitArray(data);
			result.Not();
			return result;
		}
	}
}
