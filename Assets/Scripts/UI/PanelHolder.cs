using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHolder : MonoBehaviour
{
	public GameObject craftingPanel;
	public GameObject deathPanel;
	public GameObject enginePanel;

	public void ShowDeathPanel()
	{
		deathPanel.SetActive(true);
	}

	public void ShowCraftingPanel()
	{
		craftingPanel.SetActive(true);
	}

	public void HideCraftingPanel()
	{
		craftingPanel.SetActive(false);
	}
}
