using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientHolder : MonoBehaviour
{
	List<ItemData> currentIngredients = new List<ItemData>();
	List<GameObject> currentIngredientObjects = new List<GameObject>();
	public Toggle[] toggles;
	Recipe currentRecipe;
	public Button craftingButton;
	public Image craftingButtonImage;
	public Inventory inventory;
	// Start is called before the first frame update
	void Start()
    {
		foreach (var item in toggles)
		{
			
			item.onValueChanged.AddListener((bool b) => { UpdateRecipe(b, item.GetComponent<CraftingRecipe>().recipe); });
			if (item.isOn)
			{
				currentRecipe = item.GetComponent<CraftingRecipe>().recipe;
			}
		}

		craftingButton.onClick.AddListener(Craft);
		//currentRecipe 
    }

	private void Craft()
	{
		StartCoroutine(Timer(currentRecipe.CraftingTime));
	}

	IEnumerator Timer(float craftingTime)
	{
		float t = 0;
		while (t < craftingTime)
		{
			t += Time.deltaTime;
			craftingButtonImage.fillAmount = t / craftingTime;
			yield return null;
		}

		//add the product
		inventory.AddItem(currentRecipe.Product);

		//clear the current ingredients
		for (int i = currentIngredientObjects.Count-1; i > -1; i--)
		{
			Destroy(currentIngredientObjects[i]);
		}

		currentIngredientObjects.Clear();
		currentIngredients.Clear();

		//reset the button
		craftingButton.interactable = false;
		craftingButtonImage.fillAmount = 0f;

	}

	private void UpdateRecipe(bool b, Recipe r)
	{
		if (b)
		{
			currentRecipe = r;
		}
	}

	public void Add(ItemData itemData,GameObject go)
	{
		Debug.Log("added" + itemData.name);
		currentIngredients.Add(itemData);
		currentIngredientObjects.Add(go);
		CheckIngredients();
	}

	void CheckIngredients()
	{
		Debug.Log(currentRecipe.ItemCount);
		for (int i = 0; i < currentRecipe.ItemCount; i++)
		{
			int targetCount = currentRecipe.GetNumber(i);
			int currentCount = 0;
			for (int r = 0; r < currentIngredients.Count; r++)
			{
				if (currentIngredients[i] == currentRecipe.GetItem(i))
				{
					Debug.Log("increasing count");
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

	public void Remove(ItemData itemData)
	{
		currentIngredients.Remove(itemData);
		CheckIngredients();
	}
}
