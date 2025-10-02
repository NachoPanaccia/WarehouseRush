using System;
using System.Collections.Generic;

public static class QuickSorter
{
    public static void QuickSort<T>(List<T> list, Comparison<T> comparison)
    {
        if (list == null || list.Count <= 1) return;
        QuickSort(list, 0, list.Count - 1, comparison);
    }

    private static void QuickSort<T>(List<T> list, int left, int right, Comparison<T> cmp)
    {
        int i = left;
        int j = right;
        T pivot = list[(left + right) / 2];

        while (i <= j)
        {
            while (cmp(list[i], pivot) < 0) i++;
            while (cmp(list[j], pivot) > 0) j--;
            if (i <= j)
            {
                (list[i], list[j]) = (list[j], list[i]);
                i++; j--;
            }
        }
        if (left < j) QuickSort(list, left, j, cmp);
        if (i < right) QuickSort(list, i, right, cmp);
    }
}
