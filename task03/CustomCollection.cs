using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomCollection<T> : IEnumerable<T>
{
    private readonly List<T> _items = new List<T>();

    public void Add(T item) => _items.Add(item);

    // Стандартный итератор
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Обратный итератор 
    public IEnumerable<T> GetReverseEnumerator() => 
        _items.AsEnumerable().Reverse();

    // Генератор последовательности 
    public static IEnumerable<int> GenerateSequence(int start, int count) => 
        Enumerable.Range(start, count);

    // LINQ-фильтрация и сортировка 
    public IEnumerable<T> FilterAndSort(Func<T, bool> predicate, Func<T, IComparable> keySelector) => 
        _items.Where(predicate).OrderBy(keySelector);
}
