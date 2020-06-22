using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Recipe))]
public class RecipeInspector : Editor
{
	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		Recipe recipe = target as Recipe;

		//display the crafting time
		EditorGUI.BeginChangeCheck();
		float t = EditorGUILayout.FloatField("Crafting Time", recipe.CraftingTime);

		if (EditorGUI.EndChangeCheck())
		{
			recipe.SetCraftingTime(t);
			EditorUtility.SetDirty(recipe);
		}

		EditorGUIUtility.labelWidth = 80;
		for (int i = 0; i < recipe.ItemCount; i++)
		{
			EditorGUILayout.BeginHorizontal();

			//display the number of items
			EditorGUI.BeginChangeCheck();
			int x = EditorGUILayout.IntField("Number", recipe.GetNumber(i));
			if (EditorGUI.EndChangeCheck())
			{
				recipe.SetNumber(i, x);
				EditorUtility.SetDirty(recipe);
			}

			//display the item data
			EditorGUI.BeginChangeCheck();
			ItemData data = EditorGUILayout.ObjectField("Item", recipe.GetItem(i), 
				typeof(ItemData), false) as ItemData;
			if (EditorGUI.EndChangeCheck())
			{
				recipe.SetItem(i, data);
				EditorUtility.SetDirty(recipe);
			}


			if (GUILayout.Button("Remove"))
			{
				recipe.Remove(i);
				EditorUtility.SetDirty(recipe);
			}

			EditorGUILayout.EndHorizontal();
		}

		if (GUILayout.Button("Add"))
		{
			recipe.Add();
			EditorUtility.SetDirty(recipe);
		}

		//display the product of the recipe
		EditorGUI.BeginChangeCheck();
		ItemData p = EditorGUILayout.ObjectField("Product", recipe.Product,
			typeof(ItemData), false) as ItemData;
		if (EditorGUI.EndChangeCheck())
		{
			recipe.SetProduct(p);
			EditorUtility.SetDirty(recipe);
		}
	}
}
