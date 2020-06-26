using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTrigger : MonoBehaviour
{
	public delegate void InventoryHandler(Item i);
	public event InventoryHandler ItemSelected;
	public event InventoryHandler ItemUnselected;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Item")
		{
			Debug.Log("here");
			if (ItemSelected != null)
			{
				ItemSelected(collision.GetComponent<Item>());
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
	}
}
