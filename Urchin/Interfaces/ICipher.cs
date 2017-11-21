using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urchin.Interfaces
{
    public interface ICipher
    {
        IKeySchedule KeySchedule { get; set; }
        byte[] Encrypt(byte[] plaintext);
        byte[] Decrypt(byte[] ciphertext);
        byte[] Encrypt(string plaintext);
        byte[] Decrypt(string ciphertext);
    }
}
