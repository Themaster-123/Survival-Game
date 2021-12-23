using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Rigidbody))]
public class BoundsBehavior : Behavior
{
	public Vector3 checkPosition;
	public LayerMask groundMask;
	protected Entity entity;
	protected Rigidbody rigidBody;

	protected virtual void FixedUpdate()
	{
		CheckGroundneath();
	}

	protected virtual void CheckGroundneath()
	{
		Vector3Int pos = VoxelUtilities.ToVoxelPosition(transform.position + checkPosition, entity.world);
		if (entity.world.IsVoxelSurrounded(pos))
		{
			if (Physics.Raycast(transform.position + checkPosition, Vector3.up, out RaycastHit hit, float.PositiveInfinity, groundMask) && hit.distance > .3f)
			{
				rigidBody.position = hit.point - checkPosition;
				//rigidBody.velocity = new Vector3(rigidBody.velocity.x, 9, rigidBody.velocity.z);
			} else
			{
				rigidBody.AddForce(Vector3.up * 50, ForceMode.Acceleration);
			}
		}
	}

	protected override void GetComponents()
	{
		base.GetComponents();
		entity = GetComponent<Entity>();
		rigidBody = GetComponent<Rigidbody>();
	}
}
