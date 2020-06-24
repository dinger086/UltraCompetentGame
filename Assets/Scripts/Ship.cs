using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
	SpriteRenderer sr;

	public void RegisterPlayer(PlayerScript ps)
	{
		ps.EnteredAir += OnEnteredAir;
		ps.EnteredWater += OnEnteredWater;
	}

	private void OnEnteredWater()
	{
		sr.enabled = true;
	}

	private void OnEnteredAir()
	{
		sr.enabled = false;
	}

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
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
