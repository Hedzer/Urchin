namespace Urchin.Extensions.BitArray.Reverse
{
    public static class ReverseExt
    {
        public static void Reverse(this System.Collections.BitArray instance)
        {
            int length = instance.Length;
            int mid = (length / 2);
            for (int i = 0; i < mid; i++)
            {
                bool bit = instance[i];
                instance[i] = instance[length - i - 1];
                instance[length - i - 1] = bit;
            }
        }
    }
}
