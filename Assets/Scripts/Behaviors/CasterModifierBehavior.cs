using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionBehavior))]
public class CasterModifierBehavior : ModifierBehavior
{
    public Vector3 rayPosition;
    public float maxInteractionDistance = 5;
    public LayerMask interactionMask;
    public float continuousModifySpeed = 1;
    public float continuousModifyRate = 30;

    protected Coroutine continuousModifier;
    protected DirectionBehavior directionBehavior;

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

    public virtual void AttemptModify(in Voxel voxel)
    {
        RaycastHit hit = RaycastFromHead(interactionMask);

        if (hit.collider != null)
        {
            Modify(hit.point, voxel);
        }
    }

    public virtual void StartContinuousModifying(Voxel voxel)
	{
        StopContinuousModifying();
        continuousModifier = StartCoroutine(EContinuousModify(voxel));
	}

    public virtual void StopContinuousModifying()
	{
        if (continuousModifier != null)
        {
            StopCoroutine(continuousModifier);
        }
    }

    protected virtual IEnumerator EContinuousModify(Voxel voxel)
	{
        while (true)
		{
            Voxel vox = voxel;
            vox.value = vox.value / continuousModifyRate * continuousModifySpeed;
            AttemptModify(vox);
            yield return new WaitForSeconds(1 / continuousModifyRate);
        }
	}

	protected override void GetComponents()
	{
		base.GetComponents();
        directionBehavior = GetComponent<DirectionBehavior>();
    }
}
