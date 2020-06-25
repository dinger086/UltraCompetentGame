using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
	Vector2 original;
	public GameObject craftingIngredientHolder;
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

		RectTransform invPanel = craftingIngredientHolder.GetComponent<RectTransform>();
		if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
		{
			//Debug
			transform.SetParent(craftingIngredientHolder.transform);
			craftingIngredientHolder.GetComponent<IngredientHolder>().Add(itemData);
		}
	}
}
