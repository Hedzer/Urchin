using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urchin.Extensions.IEnumerable.Swap
{
    static class SwapExt
    {
        public static void Swap<T>(this T[] instance, int indexA, int indexB)
        {
            T current = instance[indexA];
            T swap = instance[indexB];
            instance[indexA] = swap;
            instance[indexB] = current;
        }
    }
}
