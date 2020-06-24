﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
	public void OnDrop(PointerEventData eventData)
	{
		RectTransform invPanel = transform as RectTransform;

		if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel,Input.mousePosition))
		{
			//Debug
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
