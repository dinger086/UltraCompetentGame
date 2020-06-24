using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
		FindObjectOfType<PlayerScript>().EnteredAir += OnEnteredAir;
		FindObjectOfType<PlayerScript>().EnteredWater += OnEnteredWater;

		gameObject.SetActive(false);
	}

	private void OnEnteredWater()
	{
		gameObject.SetActive(false);
	}

	private void OnEnteredAir()
	{
		gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
