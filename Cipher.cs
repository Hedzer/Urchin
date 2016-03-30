using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urchin {
	public class Cipher {
		Settings Settings = new Settings();
		public Cipher(Settings settings){
			this.Settings = settings;
		}
		bool isInitialized(){
			return string.IsNullOrEmpty(Settings.key);
		}
		public byte[] encrypt(byte[] plaintext) {
			byte[] result = new byte[plaintext.Length];
			return result;
		}
		public byte[] encrypt(String plaintext) {
			byte[] result = new byte[plaintext.Length];
			return result;
		}
		public byte[] decrypt(byte[] ciphertext) {
			byte[] result = new byte[ciphertext.Length];
			return result;
		}
		public byte[] decrypt(String ciphertext) {
			byte[] result = new byte[ciphertext.Length];
			return result;
		}
	}
}
