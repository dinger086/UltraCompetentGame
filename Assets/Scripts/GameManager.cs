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

			GameObject player = Instantiate(playerPrefab, new Vector3(-4, 2.5f, 0), Quaternion.identity);

			//register with the camera
			FindObjectOfType<CameraFollow>().RegisterPlayer(player.transform);

			PlayerScript ps = player.GetComponent<PlayerScript>();

			//set up player information
			ps.swimForce = playerData.SwimSpeed * 20f;
			ps.maxInventoryCount = playerData.ItemCarryLimit;

			//again not good coding style
			ps.EnteredAir += FindObjectOfType<Show>().OnEnteredAir;
			ps.EnteredWater += FindObjectOfType<Show>().OnEnteredWater;

			//let the ship know when we enter and exit
			FindObjectOfType<Ship>().RegisterPlayer(ps);


			//set up the survival mechanics
			ps.FoodEaten += FindObjectOfType<HungerMeter>().OnFoodEaten;
			FindObjectOfType<HungerMeter>().Starved += ps.OnStarved;

			GetComponentInChildren<InventoryTrigger>().ItemSelected += ps.OnItemSelected;
			GetComponentInChildren<InventoryTrigger>().ItemUnselected += ps.OnItemUnselected;

			//not good coding, but doing this to avoid messing with script execution order or setting
			//up a static messaging class
			ps.EnteredAir += FindObjectOfType<OxygenMeter>().OnAirEntered;
			ps.EnteredWater += FindObjectOfType<OxygenMeter>().OnWaterEntered;
			FindObjectOfType<OxygenMeter>().OxygenDepleted += ps.OnOxygenDepleted;

			//Enter("water");

			FindObjectOfType<Inventory>().RegisterPlayer(ps);

			//set up crafting
			CraftingStation cs = FindObjectOfType<CraftingStation>();
			cs.foodBaseSpeed = playerData.FoodCookSpeedMultiplier;
			cs.shipBaseSpeed = playerData.ShipRoomCraftTimeMultiplier;
			cs.itemBaseSpeed = playerData.ItemCraftTimeMultiplier;


		}
	}

}
