using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStation : MonoBehaviour
{
	public GameObject enginePanel;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("here");
		if (collision.tag == "Player")
		{
			//Debug.Log("crafting enabled");
			enginePanel.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			enginePanel.SetActive(false);
			//Debug.Log("crafting disabled");
		}
	}
}
