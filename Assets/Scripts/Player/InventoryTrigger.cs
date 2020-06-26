using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTrigger : MonoBehaviour
{
	public delegate void InventoryHandler(Item i);
	public event InventoryHandler ItemSelected;
	public event InventoryHandler ItemUnselected;

	public delegate void AnimalHandler(GameObject go);
	public event AnimalHandler AnimalSelected;
	public event AnimalHandler AnimalUnselected;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Item")
		{
			//Debug.Log("here");
			if (ItemSelected != null)
			{
				ItemSelected(collision.GetComponent<Item>());
			}
		}
		else if (collision.tag == "Animal")
		{
			if (AnimalSelected != null)
			{
				AnimalSelected(collision.gameObject);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Item")
		{
			if (ItemUnselected != null)
			{
				ItemUnselected(collision.GetComponent<Item>());
			}
		}
		else if (collision.tag == "Animal")
		{
			if (AnimalUnselected != null)
			{
				AnimalUnselected(collision.gameObject);
			}
		}
	}
}
