using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
	public Vector3Int size;
	public Vector3 center;

	public void Destroy()
	{
		Destroy(gameObject);
	}
}
