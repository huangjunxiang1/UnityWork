using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class FastList<T> : IList<T> where T : unmanaged
    {
        public FastList() : this(0) { }
        public FastList(int capacity)
        {
            if (capacity < 0)
                capacity = 0;
            if (capacity > 0)
                this.array = new T[capacity];
        }
        private T[] array;
        private int _length;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _length)
                    throw new IndexOutOfRangeException();
                return array[index];
            }
            set
            {
                if (index < 0 || index >= _length)
                    throw new IndexOutOfRangeException();
                array[index] = value;
            }
        }

        public int Count
        {
            get => _length;
            set
            {
                if (value > Capacity)
                    this.Capacity = value;
                _length = value;
            }
        }
        public bool IsReadOnly => false;
        public int Capacity
        {
            get
            {
                return array == null ? 0 : array.Length;
            }
            set
            {
                if (array == null || array.Length < value)
                    Array.Resize(ref array, value);
            }
        }
        public void Add(T item)
        {
            if (array == null || _length >= array.Length)
                Array.Resize(ref array, (array == null || array.Length < 4) ? 4 : array.Length * 2);
            array[_length++] = item;
        }
        public void Clear()
        {
            _length = 0;
        }
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] destArray, int arrayIndex)
        {
            if (destArray == null)
                throw new ArgumentNullException(nameof(destArray));
            if (arrayIndex < 0 || arrayIndex > destArray.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (destArray.Length - arrayIndex < _length)
                throw new ArgumentException("Destination array is too small.");

            Array.Copy(array, 0, destArray, arrayIndex, _length);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0)
                return false;
            RemoveAt(index);
            return true;
        }

        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < _length; i++)
            {
                if (comparer.Equals(array[i], item))
                    return i;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > _length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (_length == (array?.Length ?? 0))
                Array.Resize(ref array, array == null ? 4 : array.Length * 2);

            if (index < _length)
            {
                Array.Copy(array, index, array, index + 1, _length - index);
            }
            array[index] = item;
            _length++;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (index < _length - 1)
            {
                Array.Copy(array, index + 1, array, index, _length - index - 1);
            }
            _length--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _length; i++)
                yield return array[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ref T ElementAt(int index)
        {
            if (index < 0 || index >= _length)
                throw new IndexOutOfRangeException();
            return ref array[index];
        }
        public void Reverse(int index, int count)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return;

            int actualCount = Math.Min(count, Count - index);
            int left = index;
            int right = index + actualCount - 1;
            while (left < right)
            {
                T tmp = array[left];
                array[left] = array[right];
                array[right] = tmp;
                left++;
                right--;
            }
        }
        public List<T> ToList()
        {
            List<T> list = new(Count);
            for (int i = 0; i < Count; i++)
                list.Add(array[i]);
            return list;
        }
        public void Dispose()
        {

        }
    }
}
