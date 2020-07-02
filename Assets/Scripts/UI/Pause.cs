using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
	public GameObject pausePanel;
	public bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
		Time.timeScale = 1f;
		pausePanel.SetActive(isPaused);

		GetComponent<Tutorial>().StartTutorial();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SwitchPause();
		}
    }

	public void SwitchPause()
	{
		isPaused = !isPaused;

		Time.timeScale = isPaused ? 0f : 1f;
		pausePanel.SetActive(isPaused);
	}

	public void ReturnToMain()
	{
		SceneManager.LoadScene(1,LoadSceneMode.Additive);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
