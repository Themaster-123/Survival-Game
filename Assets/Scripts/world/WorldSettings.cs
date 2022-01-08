using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World Settings", menuName = "Survival Game/World Settings")]
public class WorldSettings : ScriptableObject
{
	public float ChunkSize = 16;
	public int ChunkResolution = 32;
	public float InverseChunkResolution = 1 / 32;

	protected void OnValidate()
	{
		InverseChunkResolution = 1f / ChunkResolution;
	}
}
