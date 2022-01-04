using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridVisualizer : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1;

    public Pathfinding pathFinding;
    public Grid<PathNode> grid;

    protected const int TEXTURE_LENGTH = 5;
    protected TextMesh[,] textValues;
    protected InputMaster inputMaster;
    protected MeshFilter meshFilter;
    protected Mesh mesh;
    protected List<PathNode> path;

    public Vector3 GetWorldPosition(int x, int y)
	{
        return transform.TransformPoint(GetLocalPosition(x, y));
	}

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return GetWorldPosition(gridPosition.x, gridPosition.y);
    }

    public Vector3 GetLocalPosition(int x, int y)
	{
        return (new Vector3(x, y) - (new Vector3(width, height) / 2)) * cellSize;
    }

    public Vector2Int GetGridPosition(Vector3 position)
	{
        return (Vector2Int)Vector3Int.FloorToInt(transform.InverseTransformPoint(position / cellSize) + (new Vector3(width, height) / 2));
	}

    void Awake()
    {
        pathFinding = new Pathfinding(width, height);
        grid = pathFinding.grid;
        textValues = new TextMesh[width, height];
		grid.OnValueChangedEvent += Grid_OnValueChangedEvent;
        inputMaster = new InputMaster();
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        path = new List<PathNode>();
    }

	private void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                textValues[x, y] = DebugUtilities.CreateWorldText(grid[x, y].ToString(), this.transform, GetLocalPosition(x, y) + (new Vector3(.5f, .5f)) * cellSize, 300, Color.white, TextAnchor.MiddleCenter);
            }
        }
		inputMaster.Testing.LeftClick.performed += context => OnLeftClick();
		inputMaster.Testing.RightClick.performed += context => OnRightClick();
        UpdateVisuals();
    }

	private void OnLeftClick()
	{
        Vector2Int pos = GetGridPosition(GetWorldMousePosition());
        if (grid.IsInBounds(pos))
		{
            //grid[pos] += 1;
            List<PathNode> path = pathFinding.FindPath(Vector2Int.zero, pos);

            if (path != null)
			{
                /*                print("fadasd");
                                for (int i = 1; i < path.Count; i++)
                                {
                                    PathNode node0 = path[i-1];
                                    PathNode node1 = path[i];
                                    //Debug.DrawLine(GetWorldPosition(node0.gridPosition) + transform.TransformDirection(new Vector3(1, 1)) * .5f, GetWorldPosition(node1.gridPosition) + transform.TransformDirection(new Vector3(1, 1)) * .5f, Color.red, 2.5f);
                                }*/
                this.path = path;
			}

        }
        UpdateVisuals();
    }
    private void OnRightClick()
    {
        Vector2Int pos = GetGridPosition(GetWorldMousePosition());
        if (grid.IsInBounds(pos))
        {
            grid[pos].walkable = !grid[pos].walkable;
        }
        UpdateVisuals();
    }

    private void OnEnable()
	{
        inputMaster.Enable();
    }

	private void OnDisable()
	{
        inputMaster.Disable();
	}

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.white;
        if (Application.isPlaying)
		{
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y));
                    Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
                }
            }
            Gizmos.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
            Gizmos.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));

        }
    }

    protected void Grid_OnValueChangedEvent(int x, int y, PathNode value)
    {
        textValues[x, y].text = value.ToString();
    }

    protected virtual Vector3 GetWorldMousePosition()
	{
        return Camera.main.ScreenToWorldPoint(inputMaster.Testing.MousePosition.ReadValue<Vector2>());
	}

    protected virtual void UpdateVisuals()
	{
        Vector3[] vertices = new Vector3[4 * width * height];
        int[] triangles = new int[6 * width * height];
        Vector2[] uvs = new Vector2[4 * width * height];

        for (int x = 0; x < width; x++)
		{
            for (int y = 0; y < height; y++)
			{
                int index = (x * height + y);
                int triIndex = 6 * index;
                int index0 = index * 4;
                int index1 = index0 + 1;
                int index2 = index0 + 2;
                int index3 = index0 + 3;
                //int value = grid[x, y];

                vertices[index0] = new Vector3(x, y) * cellSize - (new Vector3(width, height) / 2) *cellSize;
                vertices[index1] = new Vector3(x, y) * cellSize + new Vector3(cellSize, 0) - (new Vector3(width, height) / 2) * cellSize;
                vertices[index2] = new Vector3(x, y) * cellSize + new Vector3(0, cellSize) - (new Vector3(width, height) / 2) * cellSize;
                vertices[index3] = new Vector3(x, y) * cellSize + new Vector3(cellSize, cellSize) - (new Vector3(width, height) / 2) * cellSize;
                triangles[triIndex] = index0;
                triangles[triIndex+1] = index2;
                triangles[triIndex+2] = index1;
                triangles[triIndex+3] = index2;
                triangles[triIndex+4] = index3;
                triangles[triIndex+5] = index1;
                if (path.Contains(grid[x, y]))
				{
					uvs[index0] = new Vector2(4f / TEXTURE_LENGTH + .5f / TEXTURE_LENGTH, 0);
					uvs[index1] = new Vector2(4f / TEXTURE_LENGTH + .5f / TEXTURE_LENGTH, 0);
					uvs[index2] = new Vector2(4f / TEXTURE_LENGTH + .5f / TEXTURE_LENGTH, 0);
					uvs[index3] = new Vector2(4f / TEXTURE_LENGTH + .5f / TEXTURE_LENGTH, 0);
				} else if (!grid[x, y].walkable)
				{
                    uvs[index0] = new Vector2(3f / TEXTURE_LENGTH + .5f / TEXTURE_LENGTH, 0);
                    uvs[index1] = new Vector2(3f / TEXTURE_LENGTH + .5f / TEXTURE_LENGTH, 0);
                    uvs[index2] = new Vector2(3f / TEXTURE_LENGTH + .5f / TEXTURE_LENGTH, 0);
                    uvs[index3] = new Vector2(3f / TEXTURE_LENGTH + .5f / TEXTURE_LENGTH, 0);
                }

            }
        }
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
	}
}
