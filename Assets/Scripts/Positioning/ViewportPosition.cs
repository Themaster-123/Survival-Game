using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportPosition : MonoBehaviour
{
	public new Camera camera;
	public Vector3 viewPointPosition;

	protected virtual void LateUpdate()
	{
		Vector3 pos = camera.ViewportToWorldPoint(viewPointPosition);
		transform.position = pos;
		transform.rotation = camera.transform.rotation;
	}
}
