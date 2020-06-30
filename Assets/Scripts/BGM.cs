using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
	[FMODUnity.EventRef]
	public string bgm;

	FMOD.Studio.EventInstance music;
    // Start is called before the first frame update
    void Start()
    {
		music = FMODUnity.RuntimeManager.CreateInstance(bgm);
		music.start();
		FindObjectOfType<SquidTimer>().KrakenAppeared += OnKrakenAppeared;
    }

	private void OnKrakenAppeared()
	{
		music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
