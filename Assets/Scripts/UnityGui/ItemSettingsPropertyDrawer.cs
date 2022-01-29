using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ItemSettings))]
public class ItemSettingsPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label.text = ((ItemType)property.FindPropertyRelative("itemType").enumValueIndex).ToString();
		EditorGUI.PropertyField(position, property, label, true);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property, label) * (property.isExpanded == true ? property.CountInProperty() + 0.5f : 1);
	}
}
