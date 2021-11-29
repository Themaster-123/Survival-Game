using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System;
using System.Runtime.InteropServices;
using System.Threading;

[StructLayout(LayoutKind.Sequential)]
unsafe public struct NativeCounter : IDisposable
{
	public int Count
	{
		get
		{
			return *m_Counter;
		}

		set
		{
			*m_Counter = value;
		}
	}
	public bool IsCreated
	{
		get { return m_Counter != null; }
	}

	[NativeDisableUnsafePtrRestriction]
	private int* m_Counter;

	Allocator m_AllocatorLabel;

	public NativeCounter(Allocator label)
	{
		m_AllocatorLabel = label;

		m_Counter = (int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>(), 4, label);

		Count = 0;
	}

	public int Increment()
	{
		return Interlocked.Increment(ref *m_Counter) - 1;
	}

	public void Dispose()
	{
		UnsafeUtility.Free(m_Counter, m_AllocatorLabel);
		m_Counter = null;
	}
}
