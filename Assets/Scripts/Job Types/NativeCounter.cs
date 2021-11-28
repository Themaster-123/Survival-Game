using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System;
using System.Runtime.InteropServices;
using System.Threading;

[StructLayout(LayoutKind.Sequential)]
[NativeContainer]
unsafe public struct NativeCounter : IDisposable
{
	public int Count
	{
		get
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
			return *m_Counter;
		}

		set
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
			*m_Counter = value;
		}
	}
	public bool IsCreated
	{
		get { return m_Counter != null; }
	}

	[NativeDisableUnsafePtrRestriction]
	private int* m_Counter;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
	private AtomicSafetyHandle m_Safety;

	[NativeSetClassTypeToNullOnSchedule]
	private DisposeSentinel m_DisposeSentinel;
#endif

	Allocator m_AllocatorLabel;

	public NativeCounter(Allocator label)
	{
		m_AllocatorLabel = label;

		m_Counter = (int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>(), 4, label);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
		DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0, m_AllocatorLabel);
#endif
		Count = 0;
	}

	public int Increment()
	{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
		AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
		return Interlocked.Increment(ref *m_Counter) - 1;
	}

	public void Dispose()
	{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
		DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
#endif

		UnsafeUtility.Free(m_Counter, m_AllocatorLabel);
		m_Counter = null;
	}
}
