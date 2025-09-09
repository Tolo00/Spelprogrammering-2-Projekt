using System;
using System.Collections.Generic;

namespace Arosoul.Essentials {

    public static class ArrayExtensions
    {
        private static readonly Random random = new Random();

        // List shuffle
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1); // Generate a random index
                // Swap elements at indices i and j
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        // Array shuffle
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1); // Generate a random index
                // Swap elements at indices i and j
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }

}