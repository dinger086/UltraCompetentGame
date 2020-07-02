using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	public GameObject tutorialPanel;
	public Text tutorialText;
	public string[] messages;
	int current = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

	public void StartTutorial()
	{
		Time.timeScale = 0f;
		tutorialPanel.SetActive(true);
		tutorialText.text = messages[current];
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void ShowNext()
	{
		current++;

		if (current >= messages.Length)
		{
			tutorialPanel.SetActive(false);
			GetComponent<Pause>().isPaused = false;
			Time.timeScale = 1f;
			return;
		}


	}
}
