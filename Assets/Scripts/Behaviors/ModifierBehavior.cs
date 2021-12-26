using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierBehavior : Behavior
{
    public Stencil stencil;

    protected Entity entity;

    // digs into terrain with the shape of the stencil
    public virtual void Modify(Vector3 position, in Voxel voxel)
    {
        Vector3Int voxelPosition = VoxelUtilities.ToVoxelPosition(position, entity.world);
        stencil.AddVoxel(voxel, voxelPosition, entity.world);
    }

    protected override void GetComponents()
    {
        base.GetComponents();
        entity = GetComponent<Entity>();
    }
}
