using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Stencil : MonoBehaviour
{
	public virtual void AddVoxel(in Voxel voxel, Vector3Int pos, World world)
	{
		LoopVoxel(voxel, pos, world, delegate (Voxel voxel, Vector3Int position, World world)
		{
			world.AddVoxel(voxel, position);
		});
	}

	public virtual void SetVoxel(in Voxel voxel, Vector3Int pos, World world)
	{
		LoopVoxel(voxel, pos, world, delegate (Voxel voxel, Vector3Int position, World world)
		{
			world.SetVoxel(voxel, position);
		});
	}

	public virtual void LoopVoxel(in Voxel voxel, Vector3Int pos, World world, Action<Voxel, Vector3Int, World> function)
	{
		function(voxel, pos, world);
	}
}
