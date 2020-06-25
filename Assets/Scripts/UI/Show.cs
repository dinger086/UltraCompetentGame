using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		

		gameObject.SetActive(false);
	}

	public void OnEnteredWater()
	{
		gameObject.SetActive(false);
	}

	public void OnEnteredAir()
	{
		gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
