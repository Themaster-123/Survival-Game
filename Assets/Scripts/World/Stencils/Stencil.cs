using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Stencil
{
	public virtual void SetVoxel(Voxel voxel, Vector3Int pos, World world)
	{
		world.SetVoxel(voxel, pos);
	}
}
