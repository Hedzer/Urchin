using System;
using System.Collections;

namespace Urchin.Interfaces
{
    public interface IKeySchedule : ICloneable
    {
        byte[] Key { get; set;  }
        byte[] IV { get; set;  }
        Int32 CurrentStep { get; }
        BitArray GetNext(int bitCount);
    }
}
