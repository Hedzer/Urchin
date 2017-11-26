using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;
using Urchin.Encoders;

namespace Urchin.Types
{
    public class EncodingRound
    {
        private List<EncoderProxy> transformations;
        private int wordSize;

        public int WordSize { get => wordSize; set => wordSize = value; }
        internal List<EncoderProxy> Transformations { get => transformations; set => transformations = value; }
    }
}
