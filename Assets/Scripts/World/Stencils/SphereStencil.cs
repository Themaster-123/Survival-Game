using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereStencil : RangeStencil
{
	public override void SetVoxel(in Voxel voxel, Vector3Int pos, World world)
	{
		LoopVoxel(voxel, pos, world, delegate (Voxel voxel, Vector3Int position, World world)
		{
			float distance = Vector3Int.Distance(position, pos);
			voxel.value *= (range - distance) - 1;
			if ((voxel.value < 0 && world.GetVoxel(position).value < 0) || voxel.value >= 0)
			{
				world.SetVoxel(voxel, position);
			}
		});
	}

	public override void AddVoxel(in Voxel voxel, Vector3Int pos, World world)
	{
		LoopVoxel(voxel, pos, world, delegate (Voxel voxel, Vector3Int position, World world)
		{
			float distance = Vector3Int.Distance(position, pos);
			voxel.value *= (range - distance);
			world.AddVoxel(voxel, position);
		});
	}

	public override void LoopVoxel(in Voxel voxel, Vector3Int pos, World world, Action<Voxel, Vector3Int, World> function)
	{
		int squareRange = Mathf.Max((int)range, 0);

		for (int x = -squareRange; x <= squareRange; x++)
		{
			for (int y = -squareRange; y <= squareRange; y++)
			{
				for (int z = -squareRange; z <= squareRange; z++)
				{
					Vector3Int localPos = new Vector3Int(x, y, z);
					Vector3Int voxelPosition = pos + localPos;
					if (Vector3.SqrMagnitude(localPos) < range * range)
					{
						function(voxel, voxelPosition, world);
					}
				}
			}
		}
	}
}
