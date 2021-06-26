using System.Collections.Generic;
using System;
using System.Linq;

public static class Ordering
{
    public static List<T> Sort<T>(
        List<T> source, Func<T, int> sortFunction, bool asc = true
    ) where T : new() {
        // used to sort the list of objects to resolve
        return asc ? source.OrderBy(x => sortFunction.Invoke(x)).ToList() : source.OrderByDescending(x => sortFunction.Invoke(x)).ToList();
    }
}
