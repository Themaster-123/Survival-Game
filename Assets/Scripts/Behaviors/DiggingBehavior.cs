using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionBehavior))]
[RequireComponent(typeof(Entity))]
public class DiggingBehavior : Behavior
{
    public Vector3 rayPosition;
    public float groundCheckRadius = .9f;
    public float maxInteractionDistance = 5;
    public LayerMask interactionMask;
    public Stencil stencil;

    protected DirectionBehavior directionBehavior;
    protected Entity entity;

    protected virtual RaycastHit RaycastFromHead(LayerMask mask)
    {
        Vector3 origin = transform.TransformPoint(directionBehavior.GetHorizontalEntityRotation() * rayPosition);
        Vector3 direction = directionBehavior.GetDirection();

        Physics.Raycast(origin, direction, out RaycastHit hit, maxInteractionDistance, mask, QueryTriggerInteraction.Ignore);

        return hit;
    }

    protected virtual RaycastHit RaycastFromHead()
    {
        return RaycastFromHead(interactionMask);
    }

    public virtual void AttemptDig()
    {
        RaycastHit hit = RaycastFromHead(interactionMask);

        if (hit.collider != null)
        {
            Dig(hit.point);
        }
    }

    // digs into terrain with the shape of the stencil
    protected virtual void Dig(Vector3 position)
    {
        Vector3Int voxelPosition = VoxelUtilities.ToVoxelPosition(position, entity.world);
        print(voxelPosition);
        stencil.AddVoxel(new Voxel(1), voxelPosition, entity.world);
    }

    protected override void GetComponents()
    {
        directionBehavior = GetComponent<DirectionBehavior>();
        entity = GetComponent<Entity>();
    }
}
