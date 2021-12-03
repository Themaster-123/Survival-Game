using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class MarchingCubesPlugin
{
	public const string NATIVE_LIB = "MarchingCubes";

	public void MarchingCubes(Vector3Int resolution, float isoLevel, in Voxel[] voxels, out Vector3[] vertices, out int[] triangles)
	{
		IntPtr verticesPtr = IntPtr.Zero;
		IntPtr trianglesPtr = IntPtr.Zero;
		IntPtr verticesVector = IntPtr.Zero;
		IntPtr trianglesVector = IntPtr.Zero;
		int length;
		mcMarchingCubes(isoLevel, resolution, voxels, ref verticesPtr, ref trianglesPtr, out length, ref verticesVector, ref trianglesVector);

		vertices = new Vector3[length];
		triangles = new int[length];

		int vector3ByteSize = Marshal.SizeOf(typeof(Vector3));

		for (int i = 0; i < length; i++)
		{
			vertices[i] = (Vector3)Marshal.PtrToStructure(verticesPtr, typeof(Vector3));
			verticesPtr += vector3ByteSize;
		}

		Marshal.Copy(trianglesPtr, triangles, 0, length);

		mcDeleteVector(verticesVector);
		mcDeleteVector(trianglesVector);
	}

	// Need the Vector Pointers to release the memory later.
	[DllImport(NATIVE_LIB)]
	protected static extern unsafe void mcMarchingCubes(float isoLevel, in Vector3Int resolution, in Voxel[] voxels, ref IntPtr vertices, ref IntPtr triangles, out int length, 
		ref IntPtr verticesVector, ref IntPtr trianglesVector);

	[DllImport(NATIVE_LIB)]
	protected static extern unsafe void mcDeleteVector(IntPtr vector);
}
