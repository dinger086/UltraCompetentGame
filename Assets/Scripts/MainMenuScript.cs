using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
	//changed
    public void PlayButton() => SceneManager.LoadScene(2, LoadSceneMode.Additive);
    public void QuitButton() => Application.Quit();
}
