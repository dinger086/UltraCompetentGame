﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
	[FMODUnity.EventRef]
	public string startSound;
	//changed
	public Animator title;
	public Animator play;
	public Animator quit;

	private void Awake()
	{

		Invoke("ShowTitle", 2f);
	}

	private void ShowTitle()
	{
		title.SetTrigger("FadeIn");
		Invoke("ShowButtons", 3f);
	}

	private void ShowButtons()
	{
		play.SetTrigger("FadeIn");
		quit.SetTrigger("FadeIn");
	}

	public void PlayButton()
	{
		FMODUnity.RuntimeManager.PlayOneShot(startSound);
		title.SetTrigger("FadeOut");
		play.SetTrigger("FadeOut");
		quit.SetTrigger("FadeOut");
		Invoke("Load", 1f);
	}

	private void Load()
	{
		SceneManager.LoadScene(2, LoadSceneMode.Additive);
	}
    public void QuitButton() => Application.Quit();
}
