using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalGrid : MonoBehaviour
{
	public bool displayPathGizmos = false;
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Pathfinding pathfinding;
	[Header("testing")]
	public Transform target;
	public Transform seeker;
	public bool calculatePath = false;

	protected float nodeDiameter;
	protected int width, height;
	protected List<PathNode> path = new List<PathNode>();

	public Vector3 GetWorldPosition(int x, int y)
	{
		return transform.TransformPoint(GetLocalPosition(x, y));
	}

	public Vector3 GetWorldPosition(Vector2Int gridPosition)
	{
		return GetWorldPosition(gridPosition.x, gridPosition.y);
	}

	public Vector3 GetCenterWorldPosition(int x, int y)
	{
		return GetWorldPosition(x, y) + transform.TransformDirection(new Vector3(.5f, 0 , .5f) * nodeRadius);
	}

	public Vector3 GetCenterWorldPosition(Vector2Int gridPosition)
	{
		return GetCenterWorldPosition(gridPosition.x, gridPosition.y);
	}

	public Vector3 GetLocalPosition(int x, int y)
	{
		return (new Vector3(x, 0, y) - (new Vector3(width, 0 , height) * .5f)) * nodeRadius;
	}

	public Vector3 GetCenterLocalPosition(int x, int y)
	{
		return GetLocalPosition(x, y) + new Vector3(.5f, 0, .5f) * nodeRadius;
	}

	public Vector2Int GetGridPositionUnclamped(Vector3 position)
	{
		Vector3Int vec3Pos = Vector3Int.FloorToInt(transform.InverseTransformPoint(position) / nodeRadius + (new Vector3(width, 0, height) * .5f));
		return new Vector2Int(vec3Pos.x, vec3Pos.z);
	}

	public Vector2Int GetGridPosition(Vector3 position)
	{
		Vector2Int clampedPos = GetGridPositionUnclamped(position);
		clampedPos.x = Mathf.Clamp(clampedPos.x, 0, width - 1);
		clampedPos.y = Mathf.Clamp(clampedPos.y, 0, height - 1);
		return clampedPos;
	}

	protected void Start()
	{
		CalculateFields();
		InitializeFields();
	}

	protected void Update()
	{
		//SetGridValues();
	}

	protected void OnDrawGizmos()
	{

		DrawGrid();
	}

	protected void OnValidate()
	{
		if (calculatePath == true)
		{
			List<PathNode> path = pathfinding.FindPath(GetGridPosition(seeker.position), GetGridPosition(target.position));
			if (path != null)
			{
				this.path = path;
			}
			calculatePath = false;
		}
	}

	protected void InitializeFields()
	{
		pathfinding = new Pathfinding(width, height);
		SetGridValues();
	}

	protected void CalculateFields()
	{
		nodeDiameter = nodeRadius * 2;
		width = Mathf.RoundToInt(gridWorldSize.x / nodeRadius);
		height = Mathf.RoundToInt(gridWorldSize.y / nodeRadius);
	}

	protected void SetGridValues()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Vector3 worldPoint = GetCenterWorldPosition(x, y);
				pathfinding.grid[x, y].walkable = !Physics.CheckSphere(worldPoint, nodeRadius * .5f, wallMask, QueryTriggerInteraction.Ignore);
			}
		}
	}

	protected void DrawGrid()
	{
		Gizmos.color = Color.white;
		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (pathfinding != null)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Vector3 worldPoint = GetCenterLocalPosition(x, y);
					Gizmos.color = pathfinding.grid[x, y].walkable ? Color.white : Color.gray;
					if (path.Contains(pathfinding.grid[x, y]))
					{
						Gizmos.color = Color.cyan;
					}
					if (!displayPathGizmos || path.Contains(pathfinding.grid[x, y]))
					{
						Gizmos.DrawCube(worldPoint, Vector3.one * (nodeRadius * .9f));
					}
				}
			}
		}
	}
}
