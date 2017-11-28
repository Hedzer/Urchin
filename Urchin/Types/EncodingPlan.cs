using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;
using Urchin.Encoders;

namespace Urchin.Types
{
    public class EncodingPlan
    {
        private List<IWordEncoder> transformations = new List<IWordEncoder>() { };
        private int wordSize;

        public int WordSize { get => wordSize; set => wordSize = value; }
        public List<IWordEncoder> Transformations { get => transformations; set => transformations = value; }
    }
}
