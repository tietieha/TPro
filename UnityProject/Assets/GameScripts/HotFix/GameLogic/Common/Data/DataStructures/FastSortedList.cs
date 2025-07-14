using System;
using System.Collections;
using System.Collections.Generic;

public class FastSortedList<T> : IEnumerable<T>
{
	public delegate int SortCompare(T source, T other);

	private const int kDefaultCapacity = 4;

	public int Count
	{
		get;
		private set;
	}

	private int Capacity
	{
		get;
		set;
	}

	private T[] m_InnerArray;

	private int m_Offset;
	private int Offset
	{
		get
		{
			return m_Offset;
		}
		set
		{
			m_Offset = value;
		}
	}
    public T this[int index]
    {
        get
        {
            return m_InnerArray[index];
        }
        set
        {
            m_InnerArray[index] = value;
        } 
    }

	public SortCompare Comparer;


	public static SortCompare CompareWithIComparable = (obj, other) =>
	{
		return ((IComparable)obj).CompareTo(other);
	};

	public FastSortedList()
	{
		Init(CompareWithIComparable, kDefaultCapacity);
	}
	public FastSortedList(SortCompare comparer)
	{
		Init(comparer, kDefaultCapacity);
	}

	public void Add(T item)
	{
		if (Count > 0)
		{
			int min = 0;
			int max = Count;

			int split = GetSplit(min, max);

			while (min < max)
			{
				int compare = Comparer(item, m_InnerArray[GetIndex(split)]);
				if (compare == 0)
				{
					min = split + 1;

					break;
				} else if (compare > 0)
				{
					min = split + 1;
				} else
				{
					max = split;
				}

				split = GetSplit(min, max);
			}

			Insert(item, min);
		} else
		{
			Insert(item, 0);
		}

		Count ++;
	}

	public T PopMin()
	{
		int index = GetIndex(0);

		T ret = m_InnerArray[index];
		m_InnerArray[index] = default(T);

		Offset ++;

		Count --;
		if (Count == 0)
			Offset = 0;

		return ret;
	}

	public T PeekMin()
	{
		return m_InnerArray[GetIndex(0)];
	}

	public T PopMax()
	{
		int index = GetIndex(-- Count);

		T ret = m_InnerArray[index];
		m_InnerArray[index] = default(T);

		if (Count == 0)
			Offset = 0;

		return ret;
	}

	public T PeekMax()
	{
		return m_InnerArray[GetIndex(Count - 1)];
	}

	public void Clear(bool fast = true)
	{
		if (fast == false)
			Array.Clear(m_InnerArray, GetIndex(0), Count);

		Count	= 0;
		Offset	= 0;

	}

	private void Init(SortCompare comparer, int capacity)
	{
		Offset		= 0;
		Comparer	= comparer;
		Capacity	= capacity;

		m_InnerArray = new T[capacity];
	}

	private int GetSplit(int min, int max)
	{
		return min + ((max - min) / 2);
	}

	private int GetIndex(int place)
	{
		return place + Offset;
	}

	private void Insert(T item, int place)
	{
		int index = GetIndex(place);

		bool forceLeftShift = false;

		bool capped = Count + Offset >= Capacity;
		if (capped)
		{
			forceLeftShift = Offset != 0;
			if (!forceLeftShift)
				CheckCapacity(Count + Offset + 1);
		}

		if (Count > 0)
		{
			int distanceToHead = Count - place;
			int distanceToTail = place + 1;

			if (forceLeftShift || Offset != 0 && (distanceToHead >= distanceToTail))
				Shift(m_InnerArray, Offset --, index, -1);
			else
				Shift(m_InnerArray, index, Count + Offset, 1);
		}

		m_InnerArray[GetIndex(place)] = item;
	}

	private void CheckCapacity(int min)
	{
		if (Capacity < min)
		{
			Capacity *= 2;
			if (Capacity < min)
				Capacity = min;

			Array.Resize(ref m_InnerArray, Capacity);
		}
	}

	private static void Shift(Array array, int min, int max, int shiftAmount)
	{
		if (shiftAmount == 0)
			return;

		Array.Copy(array, min, array, min + shiftAmount, max - min);
	}

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return m_InnerArray[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return m_InnerArray[i];
        }
    }
}
