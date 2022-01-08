using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalGrid : MonoBehaviour
{
	public bool DisplayGridGizmos = false;
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Pathfinding pathfinding;

	[Header("testing")]
	public Transform target;
	public Transform seeker;
	public bool calculatePath = false;
	protected Vector3[] path = new Vector3[0];

	protected float nodeDiameter;
	protected int width, height;

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

	protected void Awake()
	{
		CalculateFields();
		InitializeFields();
	}

	protected void OnDrawGizmos()
	{

		DrawGrid();
	}

	protected void OnValidate()
	{
		if (calculatePath == true)
		{
			path = PathRequestManager.GetPath(seeker.position, target.position);
			calculatePath = false;
		}
	}

	protected void InitializeFields()
	{
		Grid<PathNode> grid = new Grid<PathNode>(width, height, (Grid<PathNode> grid, int x, int y) => new PathNode(grid, new Vector2Int(x, y)));
		pathfinding = new Pathfinding(grid);
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

		if (pathfinding != null && DisplayGridGizmos)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Vector3 worldPoint = GetCenterLocalPosition(x, y);
					Gizmos.color = pathfinding.grid[x, y].walkable ? Color.white : Color.gray;
					Gizmos.DrawCube(worldPoint, Vector3.one * (nodeRadius * .9f));
				}
			}
		}

		Gizmos.color = Color.cyan;
		for (int i = 1; i < path.Length; i++)
		{
			Gizmos.DrawLine(path[i - 1], path[i]);
		}
	}
}
