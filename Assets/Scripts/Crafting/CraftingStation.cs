using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftingStation : MonoBehaviour
{
	public float foodBaseSpeed = 1f;
	public float itemBaseSpeed = 1f;
	public float shipBaseSpeed = 1f;
	public GameObject craftingPanel;

	// Start is called before the first frame update
	void Start()
    {

    }

	private void Craft()
	{
		
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("here");
		if (collision.tag == "Player")
		{
			Debug.Log("crafting enabled");
			craftingPanel.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			craftingPanel.SetActive(false);
			Debug.Log("crafting disabled");
		}
	}
}
