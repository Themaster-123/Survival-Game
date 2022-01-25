using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUtilities
{
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), float fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, float fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        gameObject.transform.localScale = Vector3.one * (1f / (500f - Mathf.Min(fontSize, 499f)));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = 500;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static void DrawPath(Vector3[] path)
	{
        if (path == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.identity;
        for (int i = 1; i < path.Length; i++)
        {
            Gizmos.DrawLine(path[i - 1], path[i]);
        }
    }
}
