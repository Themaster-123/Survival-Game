using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionBehavior))]
[RequireComponent(typeof(InteractableBehavior))]
public class CasterModifierBehavior : ModifierBehavior
{
    public float maxInteractionDistance = 5;
    public LayerMask interactionMask;
    public float continuousModifySpeed = 1;
    public float continuousModifyRate = 30;

    protected Coroutine continuousModifier;
    protected DirectionBehavior directionBehavior;
    protected InteractableBehavior interactableBehavior;

    public virtual void AttemptModify(in Voxel voxel)
    {
        RaycastHit hit = CastModiferRay();

        if (hit.collider != null)
        {
            Modify(hit.point, voxel);
        }
    }

    public virtual void StartContinuousModifying(Voxel voxel)
	{
        StopContinuousModifying();
        continuousModifier = StartCoroutine(ContinuousModify(voxel));
	}

    public virtual void StopContinuousModifying()
	{
        if (continuousModifier != null)
        {
            StopCoroutine(continuousModifier);
        }
    }

    protected virtual RaycastHit CastModiferRay()
    {
        return interactableBehavior.Raycast(interactionMask, maxInteractionDistance);
    }

    protected virtual IEnumerator ContinuousModify(Voxel voxel)
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
        interactableBehavior = GetComponent<InteractableBehavior>();
    }
}
