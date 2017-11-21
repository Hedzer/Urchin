using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urchin.Interfaces
{
    public interface IKeySchedule : ICloneable
    {
        byte[] Key { get; set;  }
        byte[] IV { get; set;  }
        Int32 CurrentStep { get; }
        BitArray GetNext(int bitCount);
        IKeySchedule CreateInstance();
    }
}
