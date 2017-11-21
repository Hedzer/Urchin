using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Abstracts;
using Urchin.Interfaces;
using System.Security.Cryptography;

namespace Urchin
{
    class KeyScheduler : KeySchedule
    {
        private int currentStep = 0;
        private List<byte> buffer = new List<byte> { };
        private static HashAlgorithm[] algorithms = new HashAlgorithm[]
        {
            new MD5Cng(),
            new SHA1Managed(),
            new SHA256Managed(),
            new SHA384Managed(),
            new SHA512Managed(),
            new RIPEMD160Managed(),
        };
        private HashAlgorithm[] hashes = algorithms;


        public override int CurrentStep => currentStep;

        public override BitArray GetNext(int bitCount)
        {
            currentStep++;
            throw new NotImplementedException();
        }

        public override IKeySchedule CreateInstance()
        {
            return new KeyScheduler();
        }

        public override object Clone()
        {
            KeyScheduler clone = (KeyScheduler)CreateInstance();
            clone.Key = Key;
            clone.IV = IV;
            clone.currentStep = currentStep;
            return clone;
        }
    }
}
