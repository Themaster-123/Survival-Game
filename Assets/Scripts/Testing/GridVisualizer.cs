using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridVisualizer : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1;

    public  Grid<int> grid;
    protected TextMesh[,] textValues;
    protected InputMaster inputMaster;
    protected MeshFilter meshFilter;
    protected Mesh mesh;

    public Vector3 GetWorldPosition(int x, int y)
	{
        return transform.TransformPoint(GetLocalPosition(x, y));
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
        grid = new Grid<int>(width, height);
        textValues = new TextMesh[width, height];
		grid.OnValueChangedEvent += Grid_OnValueChangedEvent;
        inputMaster = new InputMaster();
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
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
        UpdateVisuals();
    }

	private void OnLeftClick()
	{
        Vector2Int pos = GetGridPosition(GetWorldMousePosition());
        if (grid.IsInBounds(pos))
		{
            grid[pos] += 1;
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

    protected void Grid_OnValueChangedEvent(int x, int y, int value)
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
                int value = grid[x, y];

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
                uvs[index0] = new Vector2(value / 5f + .5f / 5f, 0);
                uvs[index1] = new Vector2(value / 5f + .5f / 5f, 0);
                uvs[index2] = new Vector2(value / 5f + .5f / 5f, 0);
                uvs[index3] = new Vector2(value / 5f + .5f / 5f, 0);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
	}
}
