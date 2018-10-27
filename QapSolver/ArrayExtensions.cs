using System;
using System.Runtime.InteropServices;

namespace ArrayExtensions
{
  static class ArrayExtension
  {
    public static void Shuffle(this int[] array)
    {
      Random random = new Random();
      int n = array.Length;

      while (n > 1)
      {
        n--;
        int i = random.Next(n + 1);
        int tmp = array[i];
        array[i] = array[n];
        array[n] = tmp;
      }
    }

    public static void Swap(this int[] array, int i, int j)
    {
      int tmp = array[i];
      array[i] = array[j];
      array[j] = tmp;
    }
  }
}