﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Urchin.Extensions.BitArray.Words
{
    public static class Words
    {
        public static List<System.Collections.BitArray> ToWords(this System.Collections.BitArray instance, int bitsPerWord)
        {
            List<System.Collections.BitArray> result = new List<System.Collections.BitArray> { };
            int wordCount = (int)Math.Ceiling((double)instance.Length / bitsPerWord);
            int lastBlockSize = instance.Length % bitsPerWord;
            int offset = 0;
            for (int i = 0; i < wordCount; i++)
            {
                bool isLastWord = (i == wordCount - 1);
                int wordSize = isLastWord ? lastBlockSize : bitsPerWord;
                System.Collections.BitArray word = new System.Collections.BitArray(wordSize);
                for (int position = 0; position < wordSize; position++)
                {
                    word[position] = instance[offset];
                    offset++;
                }
                result.Add(word);
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

        public delegate void EachItemAction<ItemType>(ItemType item, int index);
        public static void EachWord(this List<System.Collections.BitArray> instance, EachItemAction<System.Collections.BitArray> action)
        {
            int wordCount = instance.Count;
            for (int index = 0; index < wordCount; index++)
            {
                action(instance[index], index);                
            }
        }

        public static void EachBit(this System.Collections.BitArray instance, EachItemAction<bool> action)
        {
            int bitCount = instance.Length;
            for (int index = 0; index < bitCount; index++)
            {
                action(instance.Get(index), index);
            }
        }
    }
}