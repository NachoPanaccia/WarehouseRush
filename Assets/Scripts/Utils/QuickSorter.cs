using System;
using System.Collections.Generic;

public static class QuickSorter
{
    public static void QuickSort<T>(List<T> list, Comparison<T> cmp)
    {
        if (list == null || list.Count < 2) return;
        QuickSort(list, 0, list.Count - 1, cmp);
    }
    private static void QuickSort<T>(List<T> l, int left, int right, Comparison<T> cmp)
    {
        if (left >= right) return;
        int p = Partition(l, left, right, cmp);
        QuickSort(l, left, p - 1, cmp);
        QuickSort(l, p + 1, right, cmp);
    }
    private static int Partition<T>(List<T> l, int left, int right, Comparison<T> cmp)
    {
        var pivot = l[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
            if (cmp(l[j], pivot) <= 0) { i++; (l[i], l[j]) = (l[j], l[i]); }
        (l[i + 1], l[right]) = (l[right], l[i + 1]);
        return i + 1;
    }
}
