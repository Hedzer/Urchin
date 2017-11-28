
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Type = System.Collections;

namespace Urchin.Extensions.BitArray.Words
{
    public static class Words
    {
        public static List<Type.BitArray> ToWords(this Type.BitArray instance, int bitsPerWord)
        {
            List<Type.BitArray> result = new List<Type.BitArray> { };
            int wordCount = (int)Math.Ceiling((double)instance.Length / bitsPerWord);
            int lastBlockSize = instance.Length % bitsPerWord;
            int offset = 0;
            for (int i = 0; i < wordCount; i++)
            {
                bool isLastWord = (i == wordCount - 1);
                int wordSize = isLastWord  && lastBlockSize !=0 ? lastBlockSize : bitsPerWord;
                Type.BitArray word = new Type.BitArray(wordSize);
                for (int position = 0; position < wordSize; position++)
                {
                    word[position] = instance[offset];
                    offset++;
                }
                result.Add(word);
            }

            return result;
        }
        public static Type.BitArray ToBitArray(this List<Type.BitArray> instance)
        {
            List<bool> values = new List<bool> { };
            instance.ForEach((System.Collections.BitArray item) => {
                foreach (bool value in item) values.Add(value);
            });

            return new System.Collections.BitArray(values.ToArray());
        }

        public delegate void EachItemAction<ItemType>(ItemType item, int index);
        public static void EachWord(this List<Type.BitArray> instance, EachItemAction<Type.BitArray> action)
        {
            int wordCount = instance.Count;
            for (int index = 0; index < wordCount; index++)
            {
                action(instance[index], index);                
            }
        }

        public static void EachBit(this Type.BitArray instance, EachItemAction<bool> action)
        {
            int bitCount = instance.Length;
            for (int index = 0; index < bitCount; index++)
            {
                action(instance.Get(index), index);
            }
        }
    }
}