using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urchin.Interfaces;
using Urchin.Encoders;

namespace Urchin.Types
{
    class RoundSnapshot
    {
        public List<EncoderState> Transformations;
        public int WordSize;
    }
}
