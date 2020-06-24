using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	PlayerData playerData;
	public GameObject playerPrefab;
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

			GameObject player = Instantiate(playerPrefab, new Vector3(0, 1.1f, 0), Quaternion.identity);
			PlayerScript ps = player.GetComponent<PlayerScript>();

			ps.swimForce = playerData.SwimSpeed;
			ps.maxInventoryCount = playerData.ItemCarryLimit;

			//again not good coding style
			ps.EnteredAir += FindObjectOfType<Show>().OnEnteredAir;
			ps.EnteredWater += FindObjectOfType<Show>().OnEnteredWater;

			ps.FoodEaten += FindObjectOfType<HungerMeter>().OnFoodEaten;
			FindObjectOfType<HungerMeter>().Starved += ps.OnStarved;


			CraftingStation cs = FindObjectOfType<CraftingStation>();
			cs.foodBaseSpeed = playerData.FoodCookSpeedMultiplier;
			cs.shipBaseSpeed = playerData.ShipRoomCraftTimeMultiplier;
			cs.itemBaseSpeed = playerData.ItemCraftTimeMultiplier;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
