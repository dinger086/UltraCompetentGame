using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
	public Button mainMenu;
	public Button restart;

	public void OnPlayerDeath()
	{
		Invoke("Show", 1f);
	}

	private void Show()
	{
		gameObject.SetActive(true);
	}
}
