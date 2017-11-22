
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Urchin.Extensions.BitArray.Swap
{
    static class SwapExt
    {
        public static void Swap(this System.Collections.BitArray instance, int indexA, int indexB)
        {
            bool current = instance.Get(indexA);
            bool swap = instance.Get(indexB);
            instance.Set(indexA, swap);
            instance.Set(indexB, current);
        }
    }
}