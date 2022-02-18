using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BuildingDatabase : MonoBehaviour
{
	public static BuildingDatabase instance;

	[SerializeField]
	protected BuildingSettings[] buildingDatabase;
	protected Dictionary<BuildingType, Building> buildings;

	public static Building GetBuilding(BuildingType type)
	{
		return instance.buildings[type];
	}

	protected void Awake()
	{
		CreateDatabase();
		SetInstance();
	}

	protected void CreateDatabase()
	{
		buildings = new Dictionary<BuildingType, Building>();
		for (int i = 0; i < buildingDatabase.Length; i++)
		{
			buildings.Add(buildingDatabase[i].buildingType, buildingDatabase[i].prefab);
		}
	}

	protected void SetInstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}

[Serializable]
public struct BuildingSettings
{
	public BuildingType buildingType;
	public Building prefab;
}
