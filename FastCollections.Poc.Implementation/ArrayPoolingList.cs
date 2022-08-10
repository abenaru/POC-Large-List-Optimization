// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// NOTE: THIS WAS MOSTLY COPIED FROM `dotnet/runtime` FOR DEMO PURPOSES
// The parts that were copied are just throwing exceptions and not doing anything else.

using System.Buffers;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
// ReSharper disable UnusedMember.Local

namespace FastCollections.Poc.Implementation;

[SuppressMessage(
    "ReSharper",
    "MemberCanBePrivate.Global",
    Justification = "Mostly just copy-pasted from core libraries for demo purposes")]
public class ArrayPoolingList<T> : IList<T>, IList, IReadOnlyList<T>, IDisposable
{
    private const int DefaultCapacity = 4;

    // Do we want to support user-provided pools?
    private static readonly ArrayPool<T> Pool = ArrayPool<T>.Shared;

    private T[] _items;
    private int _version;

    private static readonly T[] EmptyArray = Array.Empty<T>();

    // Constructs a List. The list is initially empty and has a capacity
    // of zero. Upon adding the first element to the list the capacity is
    // increased to DefaultCapacity, and then increased in multiples of two
    // as required.
    public ArrayPoolingList()
    {
        _items = EmptyArray;
    }

    // Constructs a List with a given initial capacity. The list is
    // initially empty, but will have room for the given number of elements
    // before any reallocations are required.
    //
    public ArrayPoolingList(int capacity)
    {
        _items = capacity switch
        {
            < 0 => throw new NotImplementedException( /* TODO */),
            0 => throw new NotImplementedException( /* TODO */),
            _ => Pool.Rent(capacity)
        };
    }

    ~ArrayPoolingList() => Dispose(); // For backwards compatibility maybe?

    // Gets and sets the capacity of this list.  The capacity is the size of
    // the internal array used to hold items.  When set, the internal
    // array of the list is reallocated to the given capacity.
    //
    public int Capacity
    {
        get => _items.Length;
        set
        {
            if (value < Count)
            {
                throw new NotImplementedException( /* TODO */);
            }

            if (value == _items.Length) return;
            if (value > 0)
            {
                var newItems = Pool.Rent(value);
                if (Count > 0)
                {
                    Array.Copy(_items, newItems, Count);
                }

                Dispose();
                _items = newItems;
            }
            else
            {
                _items = EmptyArray;
            }
        }
    }

    // Read-only property describing how many elements are in the List.
    public int Count { get; private set; }

    bool IList.IsFixedSize => throw new NotImplementedException( /* TODO */);

    // Is this List read-only?
    bool ICollection<T>.IsReadOnly => throw new NotImplementedException( /* TODO */);

    bool IList.IsReadOnly => throw new NotImplementedException( /* TODO */);

    // Is this List synchronized (thread-safe)?
    bool ICollection.IsSynchronized => throw new NotImplementedException( /* TODO */);

    // Synchronization root for this object.
    object ICollection.SyncRoot => throw new NotImplementedException( /* TODO */);

    // Sets or Gets the element at the given index.
    public T this[int index]
    {
        get => throw new NotImplementedException( /* TODO */);
        set => throw new NotImplementedException( /* TODO */);
    }

    private static bool IsCompatibleObject(object? value) => throw new NotImplementedException( /* TODO */);

    object? IList.this[int index]
    {
        get => throw new NotImplementedException( /* TODO */);
        set => throw new NotImplementedException( /* TODO */);
    }

    // Adds the given object to the end of this list. The size of the list is
    // increased by one. If required, the capacity of the list is doubled
    // before adding the new element.
    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        _version++;
        var array = _items;
        var size = Count;
        if ((uint)size < (uint)array.Length)
        {
            Count = size + 1;
            array[size] = item;
        }
        else
        {
            AddWithResize(item);
        }
    }

    // Non-inline from List.Add to improve its code quality as uncommon path
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddWithResize(T item)
    {
        Debug.Assert(Count == _items.Length);
        var size = Count;
        Grow(size + 1);
        Count = size + 1;
        _items[size] = item;
    }

    int IList.Add(object? item) => throw new NotImplementedException( /* TODO */);

    // Adds the elements of the given collection to the end of this list. If
    // required, the capacity of the list is increased to twice the previous
    // capacity or the new size, whichever is larger.
    //
    public void AddRange(IEnumerable<T> collection) => InsertRange(Count, collection);

    public ReadOnlyCollection<T> AsReadOnly() => throw new NotImplementedException( /* TODO */);

    // Searches a section of the list for a given element using a binary search
    // algorithm. Elements of the list are compared to the search value using
    // the given IComparer interface. If comparer is null, elements of
    // the list are compared to the search value using the IComparable
    // interface, which in that case must be implemented by all elements of the
    // list and the given search value. This method assumes that the given
    // section of the list is already sorted; if this is not the case, the
    // result will be incorrect.
    //
    // The method returns the index of the given value in the list. If the
    // list does not contain the given value, the method returns a negative
    // integer. The bitwise complement operator (~) can be applied to a
    // negative result to produce the index of the first element (if any) that
    // is larger than the given search value. This is also the index at which
    // the search value should be inserted into the list in order for the list
    // to remain sorted.
    //
    // The method uses the Array.BinarySearch method to perform the
    // search.
    //
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer) =>
        throw new NotImplementedException( /* TODO */);

    public int BinarySearch(T item) => BinarySearch(0, Count, item, null);

    public int BinarySearch(T item, IComparer<T>? comparer) => BinarySearch(0, Count, item, comparer);

    // Clears the contents of List.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => throw new NotImplementedException( /* TODO */);

    // Contains returns true if the specified element is in the List.
    // It does a linear, O(n) search.  Equality is determined by calling
    // EqualityComparer<T>.Default.Equals().
    //
    public bool Contains(T item) => throw new NotImplementedException( /* TODO */);

    bool IList.Contains(object? item) => throw new NotImplementedException( /* TODO */);

    public ArrayPoolingList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) =>
        throw new NotImplementedException( /* TODO */);

    // Copies this List into array, which must be of a
    // compatible array type.
    public void CopyTo(T[] array) => throw new NotImplementedException( /* TODO */);

    // Copies this List into array, which must be of a
    // compatible array type.
    void ICollection.CopyTo(Array array, int arrayIndex) => throw new NotImplementedException( /* TODO */);

    // Copies a section of this list to the given array at the given index.
    //
    // The method uses the Array.Copy method to copy the elements.
    //
    public void CopyTo(int index, T[] array, int arrayIndex, int count) =>
        throw new NotImplementedException( /* TODO */);

    public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException( /* TODO */);

    /// <summary>
    /// Ensures that the capacity of this list is at least the specified <paramref name="capacity"/>.
    /// If the current capacity of the list is less than specified <paramref name="capacity"/>,
    /// the capacity is increased by continuously twice current capacity until it is at least the specified <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">The minimum capacity to ensure.</param>
    /// <returns>The new capacity of this list.</returns>
    public int EnsureCapacity(int capacity)
    {
        if (capacity < 0)
        {
            throw new NotImplementedException( /* TODO */);
        }

        if (_items.Length >= capacity)
        {
            return _items.Length;
        }

        Grow(capacity);
        _version++;

        return _items.Length;
    }

    /// <summary>
    /// Increase the capacity of this list to at least the specified <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">The minimum capacity to ensure.</param>
    private void Grow(int capacity)
    {
        Debug.Assert(_items.Length < capacity);

        var newCapacity = _items.Length == 0 ? DefaultCapacity : 2 * _items.Length;

        // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
        // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
        if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

        // If the computed capacity is still less than specified, set to the original argument.
        // Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
        if (newCapacity < capacity) newCapacity = capacity;

        Capacity = newCapacity;
    }

    public bool Exists(Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public T? Find(Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public ArrayPoolingList<T> FindAll(Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public int FindIndex(Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public int FindIndex(int startIndex, Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public int FindIndex(int startIndex, int count, Predicate<T> match) =>
        throw new NotImplementedException( /* TODO */);

    public T? FindLast(Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public int FindLastIndex(Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public int FindLastIndex(int startIndex, Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public int FindLastIndex(int startIndex, int count, Predicate<T> match) =>
        throw new NotImplementedException( /* TODO */);

    public void ForEach(Action<T> action) => throw new NotImplementedException( /* TODO */);

    // Returns an enumerator for this list with the given
    // permission for removal of elements. If modifications made to the list
    // while an enumeration is in progress, the MoveNext and
    // GetObject methods of the enumerator will throw an exception.
    //
    public Enumerator GetEnumerator()
        => new Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator()
        => new Enumerator(this);

    public ArrayPoolingList<T> GetRange(int index, int count)
    {
        if (index < 0)
        {
            throw new NotImplementedException( /* TODO */);
        }

        if (count < 0)
        {
            throw new NotImplementedException( /* TODO */);
        }

        if (Count - index < count)
        {
            throw new NotImplementedException( /* TODO */);
        }

        var naivePoolingList = new ArrayPoolingList<T>(count);
        Array.Copy(_items, index, naivePoolingList._items, 0, count);
        naivePoolingList.Count = count;
        return naivePoolingList;
    }

    // Returns the index of the first occurrence of a given value in a range of
    // this list. The list is searched forwards from beginning to end.
    // The elements of the list are compared to the given value using the
    // Object.Equals method.
    //
    // This method uses the Array.IndexOf method to perform the
    // search.
    //
    public int IndexOf(T item) => throw new NotImplementedException( /* TODO */);

    int IList.IndexOf(object? item) => throw new NotImplementedException( /* TODO */);

    // Returns the index of the first occurrence of a given value in a range of
    // this list. The list is searched forwards, starting at index
    // index and ending at count number of elements. The
    // elements of the list are compared to the given value using the
    // Object.Equals method.
    //
    // This method uses the Array.IndexOf method to perform the
    // search.
    //
    public int IndexOf(T item, int index) => throw new NotImplementedException( /* TODO */);

    // Returns the index of the first occurrence of a given value in a range of
    // this list. The list is searched forwards, starting at index
    // index and upto count number of elements. The
    // elements of the list are compared to the given value using the
    // Object.Equals method.
    //
    // This method uses the Array.IndexOf method to perform the
    // search.
    //
    public int IndexOf(T item, int index, int count) => throw new NotImplementedException( /* TODO */);

    // Inserts an element into this list at a given index. The size of the list
    // is increased by one. If required, the capacity of the list is doubled
    // before inserting the new element.
    //
    public void Insert(int index, T item)
    {
        // Note that insertions at the end are legal.
        if ((uint)index > (uint)Count)
        {
            throw new NotImplementedException( /* TODO */);
        }

        if (Count == _items.Length) Grow(Count + 1);
        if (index < Count)
        {
            Array.Copy(_items, index, _items, index + 1, Count - index);
        }

        _items[index] = item;
        Count++;
        _version++;
    }

    void IList.Insert(int index, object? item)
    {
        if (item is not (T or null))
        {
            throw new NotImplementedException( /* TODO */);
        }

        Insert(index, (T)item!);
    }

    // Inserts the elements of the given collection at a given index. If
    // required, the capacity of the list is increased to twice the previous
    // capacity or the new size, whichever is larger.  Ranges may be added
    // to the end of the list by setting index to the List's size.
    //
    public void InsertRange(int index, IEnumerable<T> collection) => throw new NotImplementedException( /* TODO */);

    // Returns the index of the last occurrence of a given value in a range of
    // this list. The list is searched backwards, starting at the end
    // and ending at the first element in the list. The elements of the list
    // are compared to the given value using the Object.Equals method.
    //
    // This method uses the Array.LastIndexOf method to perform the
    // search.
    //
    public int LastIndexOf(T item) => throw new NotImplementedException( /* TODO */);

    // Returns the index of the last occurrence of a given value in a range of
    // this list. The list is searched backwards, starting at index
    // index and ending at the first element in the list. The
    // elements of the list are compared to the given value using the
    // Object.Equals method.
    //
    // This method uses the Array.LastIndexOf method to perform the
    // search.
    //
    public int LastIndexOf(T item, int index) => throw new NotImplementedException( /* TODO */);

    // Returns the index of the last occurrence of a given value in a range of
    // this list. The list is searched backwards, starting at index
    // index and upto count elements. The elements of
    // the list are compared to the given value using the Object.Equals
    // method.
    //
    // This method uses the Array.LastIndexOf method to perform the
    // search.
    //
    public int LastIndexOf(T item, int index, int count) => throw new NotImplementedException( /* TODO */);

    // Removes the element at the given index. The size of the list is
    // decreased by one.
    public bool Remove(T item) => throw new NotImplementedException( /* TODO */);

    void IList.Remove(object? item) => throw new NotImplementedException( /* TODO */);

    // This method removes all items which matches the predicate.
    // The complexity is O(n).
    public int RemoveAll(Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    // Removes the element at the given index. The size of the list is
    // decreased by one.
    public void RemoveAt(int index) => throw new NotImplementedException( /* TODO */);

    // Removes a range of elements from this list.
    public void RemoveRange(int index, int count) => throw new NotImplementedException( /* TODO */);

    // Reverses the elements in this list.
    public void Reverse() => throw new NotImplementedException( /* TODO */);

    // Reverses the elements in a range of this list. Following a call to this
    // method, an element in the range given by index and count
    // which was previously located at index i will now be located at
    // index index + (index + count - i - 1).
    //
    public void Reverse(int index, int count) => throw new NotImplementedException( /* TODO */);

    // Sorts the elements in this list.  Uses the default comparer and
    // Array.Sort.
    public void Sort() => throw new NotImplementedException( /* TODO */);

    // Sorts the elements in this list.  Uses Array.Sort with the
    // provided comparer.
    public void Sort(IComparer<T>? comparer) => throw new NotImplementedException( /* TODO */);

    // Sorts the elements in a section of this list. The sort compares the
    // elements to each other using the given IComparer interface. If
    // comparer is null, the elements are compared to each other using
    // the IComparable interface, which in that case must be implemented by all
    // elements of the list.
    //
    // This method uses the Array.Sort method to sort the elements.
    //
    public void Sort(int index, int count, IComparer<T>? comparer) => throw new NotImplementedException( /* TODO */);

    public void Sort(Comparison<T> comparison) => throw new NotImplementedException( /* TODO */);

    // ToArray returns an array containing the contents of the List.
    // This requires copying the List, which is an O(n) operation.
    public T[] ToArray() => throw new NotImplementedException( /* TODO */);

    // Sets the capacity of this list to the size of the list. This method can
    // be used to minimize a list's memory overhead once it is known that no
    // new elements will be added to the list. To completely clear a list and
    // release all memory referenced by the list, execute the following
    // statements:
    //
    // list.Clear();
    // list.TrimExcess();
    //
    public void TrimExcess() => throw new NotImplementedException( /* TODO */);

    public bool TrueForAll(Predicate<T> match) => throw new NotImplementedException( /* TODO */);

    public struct Enumerator : IEnumerator<T>
    {
        private readonly ArrayPoolingList<T> _arrayPoolingList;
        private int _index;
        private readonly int _version;

        internal Enumerator(ArrayPoolingList<T> arrayPoolingList)
        {
            _arrayPoolingList = arrayPoolingList;
            _index = 0;
            _version = arrayPoolingList._version;
            Current = default!;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            var localNaivePoolingList = _arrayPoolingList;

            if (_version != localNaivePoolingList._version || ((uint)_index >= (uint)localNaivePoolingList.Count))
            {
                return MoveNextRare();
            }

            Current = localNaivePoolingList._items[_index];
            _index++;
            return true;
        }

        private bool MoveNextRare()
        {
            if (_version != _arrayPoolingList._version)
            {
                throw new NotImplementedException( /* TODO */);
            }

            _index = _arrayPoolingList.Count + 1;
            Current = default!;
            return false;
        }

        public T Current { get; private set; }

        object? IEnumerator.Current
        {
            get
            {
                if (_index == 0 || _index == _arrayPoolingList.Count + 1)
                {
                    throw new NotImplementedException( /* TODO */);
                }

                return Current;
            }
        }

        void IEnumerator.Reset()
        {
            if (_version != _arrayPoolingList._version)
            {
                throw new NotImplementedException( /* TODO */);
            }

            _index = 0;
            Current = default!;
        }
    }

    public void Dispose()
    {
        if (ReferenceEquals(_items, EmptyArray))
        {
            return;
        }

        Pool.Return(_items, true);
        _items = EmptyArray;

        GC.SuppressFinalize(this);
    }
}