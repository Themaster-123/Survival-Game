using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SquareStencil : RangeStencil
{
	public override void SetVoxel(Voxel voxel, Vector3Int pos, World world)
	{
		int squareRange = Mathf.Max((int)range, 1);

		for (int x = -squareRange; x <= squareRange; x++)
		{
			for (int y = -squareRange; y <= squareRange; y++)
			{
				for (int z = -squareRange; z <= squareRange; z++)
				{
					Vector3Int voxelPosition = pos + new Vector3Int(x, y, z);
					world.SetVoxel(voxel, voxelPosition);
				}
			}
		}
	}
}
