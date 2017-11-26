
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Urchin.Extensions.BitArray.Words
{
    static class Words
    {
        public static List<System.Collections.BitArray> ToWords(this System.Collections.BitArray instance, int bitsPerWord)
        {
            List<System.Collections.BitArray> result = new List<System.Collections.BitArray> { };
            int size = (int)Math.Ceiling((double)instance.Length / bitsPerWord);
            int lastBlockSize = instance.Length % bitsPerWord;
            int offset = 0;
            for (int i = 0; i < size; i++)
            {
                bool isLastWord = (i == size - 1);
                int wordSize = isLastWord ? lastBlockSize : bitsPerWord;
                System.Collections.BitArray word = new System.Collections.BitArray(wordSize);
                for (int position = 0; i < wordSize; i++)
                {
                    word[position] = instance[offset + position];
                    offset++;
                    result.Add(word);
                }
            }

            return result;
        }
        public static System.Collections.BitArray ToBitArray(this List<System.Collections.BitArray> instance)
        {
            List<bool> values = new List<bool> { };
            instance.ForEach((System.Collections.BitArray item) => {
                foreach (bool value in item) values.Add(value);
            });

            return new System.Collections.BitArray(values.ToArray());
        }
    }
}