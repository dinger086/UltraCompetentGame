using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
	Vector2 original;
	public GameObject inventory;
	public GameObject craftingIngredientHolder;
	public GameObject enginePanel;
	public ItemData itemData;
	public void OnBeginDrag(PointerEventData eventData)
	{
		original = (transform as RectTransform).anchoredPosition;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		(transform as RectTransform).anchoredPosition = original;

		RectTransform ingredientPanel = craftingIngredientHolder.GetComponent<RectTransform>();
		RectTransform inventoryPanel = inventory.GetComponent<RectTransform>();
		RectTransform engine = enginePanel.GetComponent<RectTransform>();
		if (RectTransformUtility.RectangleContainsScreenPoint(ingredientPanel, Input.mousePosition))
		{
			//Debug
			transform.SetParent(craftingIngredientHolder.transform);
			inventory.GetComponent<Inventory>().RemoveItem(gameObject, itemData);
			craftingIngredientHolder.GetComponent<IngredientHolder>().Add(itemData,gameObject);
		}
		else if (RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel, Input.mousePosition))
		{
			transform.SetParent(inventoryPanel.transform);
			craftingIngredientHolder.GetComponent<IngredientHolder>().Remove(itemData);
		}
		else if (RectTransformUtility.RectangleContainsScreenPoint(engine, Input.mousePosition))
		{
			transform.SetParent(engine.transform);
			inventory.GetComponent<Inventory>().RemoveItem(gameObject, itemData);
			if (engine.GetComponent<EnginePanel>().targetItem.name.Equals(itemData.name))
			{
				engine.GetComponent<EnginePanel>().Add(itemData);
			}
			
		}
	}
}
