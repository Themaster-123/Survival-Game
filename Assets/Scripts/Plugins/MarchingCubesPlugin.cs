using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class MarchingCubesPlugin
{
	public const string NATIVE_LIB = "Marching Cubes";

	public float Test()
	{
		return mcTest();
	}

	[DllImport(NATIVE_LIB)]
	protected static extern float mcTest();
}
