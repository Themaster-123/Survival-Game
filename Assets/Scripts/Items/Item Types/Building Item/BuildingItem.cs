using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingItem : Item
{
	public BuildingType BuildingType { get; protected set; }

	public BuildingItem(ItemSettings itemSettings, BuildingType buildingType) : base(itemSettings)
	{
		BuildingType = buildingType;
	}

	public override void Use(ItemBehavior itemBehavior, GameObject caller)
	{
		if (caller.TryGetComponent(out BuildingBehavior buildingBehavior))
		{
			buildingBehavior.Place(BuildingType);
			AnimationUtils.Rotate360(itemBehavior.transform, Vector3.right, 3);
		}
	}
}
