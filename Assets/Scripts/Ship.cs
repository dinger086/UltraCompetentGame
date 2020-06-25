using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
	SpriteRenderer sr;

	public void RegisterPlayer(PlayerScript ps)
	{
		Debug.Log("registered");
		ps.EnteredAir += OnEnteredAir;
		ps.EnteredWater += OnEnteredWater;
	}

	private void OnEnteredWater()
	{
		if (sr == null)
		{
			sr = GetComponent<SpriteRenderer>();
		}

		sr.enabled = true;
	}

	private void OnEnteredAir()
	{
		Debug.Log("called");
		if (sr == null)
		{
			sr = GetComponent<SpriteRenderer>();
		}

		sr.enabled = false;

		Debug.Log(sr.enabled);
	}

	private void Awake()
	{
		
	}
	// Start is called before the first frame update
	void Start()
    {
		sr = GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
