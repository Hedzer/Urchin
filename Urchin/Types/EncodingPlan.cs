using System.Collections.Generic;
using Urchin.Interfaces;

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
