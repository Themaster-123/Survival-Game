using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stencil
{
	public virtual void SetVoxel(Voxel voxel, Vector3Int pos, World world)
	{
		world.SetVoxel(voxel, pos);
	}
}
