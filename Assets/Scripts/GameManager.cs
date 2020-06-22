using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
		//this loads the main menu
		SceneManager.LoadScene(1, LoadSceneMode.Additive);
		SceneManager.sceneLoaded += OnSceneLoaded;
    }

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//this is the level scene,
		//we want to pick a player type
		if (scene.buildIndex == 2)
		{
			SceneManager.UnloadSceneAsync(1);
			// load them
			PlayerData[] data = Resources.LoadAll<PlayerData>("");
			playerData = data[Random.Range(0, data.Length)];
			Debug.Log(playerData.name);
			
			//we should notify the systems that require this information
			//animal/resource spawners, crafting system, the player object

		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
