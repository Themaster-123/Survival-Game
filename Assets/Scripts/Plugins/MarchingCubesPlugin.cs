using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class MarchingCubesPlugin
{
	public const string NATIVE_LIB = "MarchingCubes";

	public unsafe void Test(out Vector3[] a)
	{
		int length;
		IntPtr array = IntPtr.Zero;
		mcTest(ref array, out length);
		IntPtr orginal = array;

		a = new Vector3[length];

		int byteSize = Marshal.SizeOf(typeof(Vector3));

		for (int i = 0; i < length; i++)
		{
			a[i] = /*array[i];*/(Vector3)Marshal.PtrToStructure(array, typeof(Vector3));
			array += byteSize;
		}

		mcReleaseArray(orginal);
	}

	[DllImport(NATIVE_LIB)]
	protected static extern unsafe void mcTest(ref IntPtr array, out int length);

	[DllImport(NATIVE_LIB)]
	protected static extern unsafe void mcReleaseArray(IntPtr array);
}
