using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalGrid : MonoBehaviour
{
	public bool DisplayGridGizmos = false;
	public bool OnlyDisplayUnwalkable = false;
    public LayerMask wallMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    public Pathfinding pathfinding;

	[Header("testing")]
	public Transform target;
	public Transform seeker;
	public bool calculatePath = false;
	protected Vector3[] path = new Vector3[0];

	protected float nodeDiameter;
	protected int width, height, depth;

	public Vector3 GetWorldPosition(int x, int y, int z)
	{
		return transform.TransformPoint(GetLocalPosition(x, y, z));
	}

	public Vector3 GetWorldPosition(Vector3Int gridPosition)
	{
		return GetWorldPosition(gridPosition.x, gridPosition.y, gridPosition.z);
	}

	public Vector3 GetCenterWorldPosition(int x, int y, int z)
	{
		return GetWorldPosition(x, y, z) + transform.TransformDirection(new Vector3(.5f, .5f, .5f) * nodeRadius);
	}

	public Vector3 GetCenterWorldPosition(Vector3Int gridPosition)
	{
		return GetCenterWorldPosition(gridPosition.x, gridPosition.y, gridPosition.z);
	}

	public Vector3 GetLocalPosition(int x, int y, int z)
	{
		return (new Vector3(x, y, z) - (new Vector3(width, height , depth) * .5f)) * nodeRadius;
	}

	public Vector3 GetCenterLocalPosition(int x, int y, int z)
	{
		return GetLocalPosition(x, y, z) + new Vector3(.5f, .5f, .5f) * nodeRadius;
	}

	public Vector3Int GetGridPositionUnclamped(Vector3 position)
	{
		Vector3Int vec3Pos = Vector3Int.FloorToInt(transform.InverseTransformPoint(position) / nodeRadius + (new Vector3(width, height, depth) * .5f));
		return new Vector3Int(vec3Pos.x, vec3Pos.y, vec3Pos.z);
	}

	public Vector3Int GetGridPosition(Vector3 position)
	{
		Vector3Int clampedPos = GetGridPositionUnclamped(position);
		clampedPos.x = Mathf.Clamp(clampedPos.x, 0, width - 1);
		clampedPos.y = Mathf.Clamp(clampedPos.y, 0, height - 1);
		clampedPos.z = Mathf.Clamp(clampedPos.z, 0, depth - 1);
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
		Grid<PathNode> grid = new Grid<PathNode>(width, height, depth, (Grid<PathNode> grid, int x, int y, int z) => new PathNode(grid, new Vector3Int(x, y, z)));
		pathfinding = new Pathfinding(grid);
		SetGridValues();
	}

	protected void CalculateFields()
	{
		nodeDiameter = nodeRadius * 2;
		width = Mathf.RoundToInt(gridWorldSize.x / nodeRadius);
		height = Mathf.RoundToInt(gridWorldSize.y / nodeRadius);
		depth = Mathf.RoundToInt(gridWorldSize.z / nodeRadius);
	}

	protected void SetGridValues()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < depth; z++)
				{
					Vector3 worldPoint = GetCenterWorldPosition(x, y, z);
					pathfinding.grid[x, y, z].walkable = !Physics.CheckSphere(worldPoint, nodeRadius * .5f, wallMask, QueryTriggerInteraction.Ignore);
				}
			}
		}
	}

	protected void DrawGrid()
	{
		Gizmos.color = Color.white;
		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));

		if (pathfinding != null && DisplayGridGizmos)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					for (int z = 0; z < depth; z++)
					{
						if (!OnlyDisplayUnwalkable || !pathfinding.grid[x, y, z].walkable)
						{
							Vector3 worldPoint = GetCenterLocalPosition(x, y, z);
							Gizmos.color = pathfinding.grid[x, y, z].walkable ? Color.white : Color.gray;
							Gizmos.DrawCube(worldPoint, Vector3.one * (nodeRadius * .9f));
						}
					}
				}
			}
		}

		Gizmos.color = Color.cyan;
		Gizmos.matrix = Matrix4x4.identity;
		for (int i = 1; i < path.Length; i++)
		{
			Gizmos.DrawLine(path[i - 1], path[i]);
		}
	}
}
