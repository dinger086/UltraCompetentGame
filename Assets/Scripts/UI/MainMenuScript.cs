using System.Collections;
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
	public Animator bg;
	public Animator play;
	public Animator quit;

	private void Awake()
	{
		bg.SetFloat("Speed", .25f);
		bg.SetTrigger("FadeIn");
		Invoke("ShowTitle", 9.1f);
	}

	private void ShowTitle()
	{
		title.SetTrigger("FadeIn");
		Invoke("ShowButtons", 2.3f);
	}

	private void ShowButtons()
	{
		play.SetTrigger("FadeIn");
		quit.SetTrigger("FadeIn");

		Invoke("ActivateButtons", 1f);
	}

	private void ActivateButtons()
	{
		play.GetComponent<Button>().interactable = true;
		quit.GetComponent<Button>().interactable = true;
	}

	public void PlayButton()
	{
		FMODUnity.RuntimeManager.PlayOneShot(startSound);
		title.SetTrigger("FadeOut");
		play.SetTrigger("FadeOut");
		quit.SetTrigger("FadeOut");
		Invoke("Load", 3f);
	}

	private void Load()
	{
		SceneManager.LoadScene(2, LoadSceneMode.Additive);
	}

    public void QuitButton() => Application.Quit();
}
