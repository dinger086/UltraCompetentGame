using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distortion : MonoBehaviour
{
	MeshRenderer mr;

	public void RegisterPlayer(PlayerScript ps)
	{
		ps.EnteredAir += OnEnteredAir;
		ps.EnteredWater += OnEnteredWater;
	}

	private void OnEnteredWater()
	{
		mr.enabled = true;
	}

	private void OnEnteredAir()
	{
		mr.enabled = false;
	}

	private void Awake()
	{
		mr = GetComponent<MeshRenderer>();
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
