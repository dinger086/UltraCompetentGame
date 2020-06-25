using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe")]
public class Recipe : ScriptableObject
{
	[SerializeField]
	List<int> numbers = new List<int>();
	[SerializeField]
	List<ItemData> items = new List<ItemData>();

	[SerializeField]
	float craftingTime;

	[SerializeField]
	ItemData product;

	public ItemData Product
	{
		get { return product; }
	}

	public void SetProduct(ItemData p)
	{
		product = p;
	}

	public float CraftingTime
	{
		get { return craftingTime; }
	}

	public void SetCraftingTime(float t)
	{
		craftingTime = t;
	}

	public int ItemCount
	{
		get { return items.Count; }
	}

	public ItemData GetItem(int i)
	{
		if (i > -1 && i < items.Count)
		{
			return items[i];
		}

		return null;
	}

	public int GetNumber(int i)
	{
		if (i > -1 && i < items.Count)
		{
			return numbers[i];
		}

		return -1;
	}

	public void Add()
	{
		items.Add(null);
		numbers.Add(-1);
	}

	public void Remove(int index)
	{
		if (index > -1 && index < items.Count)
		{
			numbers.RemoveAt(index);
			items.RemoveAt(index);
		}
	}

	public void SetNumber(int index, int num)
	{
		if (index > -1 && index < items.Count)
		{
			numbers[index] = num;
		}
	}

	public void SetItem(int index, ItemData item)
	{
		if (index > -1 && index < items.Count)
		{
			items[index] = item;
		}
	}
}
