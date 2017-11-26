using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;

namespace Urchin.Encoders
{
    class EncoderProxy : IWordEncoder
    {
        private IWordEncoder encoder;
        private Type encoderType;
        public Type WordEncoder
        {
            get
            {
                return encoderType;
            }
            set
            {
                encoderType = value;
                encoder = (IWordEncoder)Activator.CreateInstance(value);
            }
        }

        public int WordSize
        {
            get
            {
                return encoder.WordSize;
            }
            set
            {
                encoder.WordSize = value;
            }
        }
        public int SeedSize
        {
            get
            {
                return encoder.SeedSize;
            }
        }

        public BitArray Seed
        {
            get
            {
                return encoder.Seed;
            }
            set
            {
                encoder.Seed = value;
            }
        }
        public BitArray Entropy
        {
            get
            {
                return encoder.Entropy;
            }
            set
            {
                encoder.Entropy = value;
            }
        }

        public BitArray Encode(BitArray word)
        {
            return encoder.Encode(word);
        }

        public BitArray Decode(BitArray word)
        {
            return encoder.Decode(word);
        }
    }
}
