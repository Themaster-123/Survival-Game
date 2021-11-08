using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World Settings", menuName = "Survival Game/World Settings")]
public class WorldSettings : ScriptableObject
{
	public float ChunkSize = 16;
	public int ChunkResolution = 32;
}
