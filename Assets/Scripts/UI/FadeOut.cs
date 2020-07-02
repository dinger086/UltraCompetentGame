using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
	SpriteRenderer sr;
	private void Awake()
	{
		FindObjectOfType<PanelHolder>().enginePanel.GetComponent<EnginePanel>().VictoryAchieved += OnVictoryAchieved;
		sr = GetComponent<SpriteRenderer>();
	}

	private void OnVictoryAchieved()
	{
		StartCoroutine(Fade());
	}

	IEnumerator Fade()
	{
		while (sr.color.a < 100f)
		{
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + Time.deltaTime);
			yield return null;
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

	internal void OnPlayerDeath()
	{
		StartCoroutine(Fade());
	}
}
