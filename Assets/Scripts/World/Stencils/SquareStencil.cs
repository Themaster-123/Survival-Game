using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SquareStencil : RangeStencil
{
	public override void LoopVoxel(in Voxel voxel, Vector3Int pos, World world, Action<Voxel, Vector3Int, World> function)
	{
		int squareRange = Mathf.Max((int)range, 0);

		for (int x = -squareRange; x <= squareRange; x++)
		{
			for (int y = -squareRange; y <= squareRange; y++)
			{
				for (int z = -squareRange; z <= squareRange; z++)
				{
					Vector3Int voxelPosition = pos + new Vector3Int(x, y, z);
					function(voxel, voxelPosition, world);
				}
			}
		}
	}
}
