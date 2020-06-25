using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public GameObject craftingIngredientHolder;
	public GameObject itemPrefab;
	List<GameObject> items = new List<GameObject>();
	int maxHorizontalItems = 8;
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
	}

	private void OnItemAdded(ItemData item)
	{
		GameObject go = Instantiate(itemPrefab, transform);
		items.Add(go);
		go.GetComponent<Drag>().craftingIngredientHolder = craftingIngredientHolder;
		go.GetComponentInChildren<Text>().text = item.name;
	}
}
