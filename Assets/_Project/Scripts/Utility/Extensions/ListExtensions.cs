using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wichtel.Extensions{
public static class ListExtensions
{
    public static void RemoveEmptyEntries<T>(this List<T> _list) where T : class
    {
        _list.RemoveAll(item => item == null);
    }
    
    public static bool AreAnyDuplicates<T>(this IEnumerable<T> _list)
    {
        var hashset = new HashSet<T>();
        return _list.Any(e => !hashset.Add(e));
    }

    public static void RemoveAllAfterIndex<T>(this List<T> _list, int _index)
    {
        int i = _index + 1;
        if (_list.Count > i)
        {
            _list.RemoveRange(i, _list.Count - i);
        }
    }

    public static void RemoveAllBeforeIndex<T>(this List<T> _list, int _index)
    {
        if (_list.Count >= _index + 1)
        {
            _list.RemoveRange(0, _index);
        }
        else
        {
            _list.Clear();
        }
    }
}
}