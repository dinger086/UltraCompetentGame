using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public GameObject craftingIngredientHolder;
	public GameObject enginePanel;
	public GameObject itemPrefab;
	List<GameObject> items = new List<GameObject>();
	int maxHorizontalItems = 8;

	public delegate void ItemHandler(ItemData item);
	public event ItemHandler ItemRemoved;
	public event ItemHandler Added;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void RegisterPlayer(PlayerScript ps)
	{
		int temp = ps.maxInventoryCount;
		if (temp > maxHorizontalItems)
		{
			GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 90f);
			temp = maxHorizontalItems;
		}

		GetComponent<GridLayoutGroup>().constraintCount = temp;
		ps.AddItem += OnItemAdded;
		ItemRemoved += ps.OnItemRemoved;
		ps.RemoveItem += OnItemRemoved;
		Added += ps.Add;
	}

	private void OnItemRemoved(ItemData item)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].GetComponent<Drag>().itemData.Equals(item))
			{
				Destroy(items[i]);
				items.RemoveAt(i);
				break;
			}
		}
	}

	private void OnItemAdded(ItemData item)
	{
		GameObject go = Instantiate(itemPrefab, transform);
		items.Add(go);
		go.GetComponent<Drag>().itemData = item;
		go.GetComponent<Drag>().craftingIngredientHolder = craftingIngredientHolder;
		go.GetComponent<Drag>().enginePanel = enginePanel;
		go.GetComponent<Drag>().inventory = gameObject;
		go.GetComponentInChildren<Text>().text = item.name;
	}

	//we only call this from outside the player script, so we need to notify the player
	public void AddItem(ItemData item)
	{
		OnItemAdded(item);
		if (Added != null)
		{
			Added(item);
		}
	}

	public void RemoveItem(GameObject item, ItemData data)
	{
		items.Remove(item);
		if (ItemRemoved != null)
		{
			ItemRemoved(data);
		}
	}
}
