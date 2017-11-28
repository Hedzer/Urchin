using System;

namespace Urchin.Extensions.BitArray.ValueEquals
{
    public static class EqualsExt
    {
        public static bool ValuesEqual(this System.Collections.BitArray instance, System.Collections.BitArray bitArray)
        {
            int length = instance.Length;
            if (bitArray == null) return false;
            if (bitArray.Length != instance.Length) return false;
            for (int i = 0; i < length; i++)
            {
                if (!Object.Equals(instance[i], bitArray[i])) return false;
            }
            return true;
        }
    }
}
