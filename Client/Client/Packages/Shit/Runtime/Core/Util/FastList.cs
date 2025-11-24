using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class FastList<T>
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

        public ref T this[int index]
        {
            get
            {
                if (index < 0 || index >= _length)
                    throw new IndexOutOfRangeException();
                return ref array[index];
            }
        }

        public int Length
        {
            get => _length;
            set
            {
                if (value > Capacity)
                    this.Capacity = value;
                _length = value;
            }
        }
        public int Capacity
        {
            get
            {
                return array == null ? 0 : array.Length;
            }
            set
            {
                if (array == null || array.Length != value)
                    Array.Resize(ref array, value);
            }
        }
        public void Add(T item)
        {
            if (array == null || _length >= array.Length)
                Array.Resize(ref array, array.Length < 4 ? 4 : array.Length * 2);
            array[_length++] = item;
        }
        public void Clear()
        {
            _length = 0;
        }
        public ref T ElementAt(int index)
        {
            if (index < 0 || index >= _length)
                throw new IndexOutOfRangeException();
            return ref array[index];
        }
        public void Dispose()
        {

        }
    }
}
