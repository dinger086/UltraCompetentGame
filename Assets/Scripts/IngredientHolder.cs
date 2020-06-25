using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientHolder : MonoBehaviour
{
	List<ItemData> currentIngredients = new List<ItemData>();
	public Toggle[] toggles;
	Recipe currentRecipe;
	public Button craftingButton;
    // Start is called before the first frame update
    void Start()
    {
		foreach (var item in toggles)
		{
			
			item.onValueChanged.AddListener((bool b) => { UpdateRecipe(b, item.GetComponent<CraftingRecipe>().recipe); });
		}
    }

	private void UpdateRecipe(bool b, Recipe r)
	{
		if (b)
		{
			currentRecipe = r;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void Add(ItemData itemData)
	{
		currentIngredients.Add(itemData);
	}

	void CheckIngredients()
	{
		for (int i = 0; i < currentRecipe.ItemCount; i++)
		{
			int targetCount = currentRecipe.GetNumber(i);
			int currentCount = 0;
			for (int r = 0; r < currentIngredients.Count; r++)
			{
				if (currentIngredients[i] == currentRecipe.GetItem(i))
				{
					currentCount++;
				}
			}

			if (currentCount != targetCount)
			{
				craftingButton.interactable = false;
				break;
			}

			craftingButton.interactable = true;
		}
	}
}
