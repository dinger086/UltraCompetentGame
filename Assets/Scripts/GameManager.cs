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

			GameObject player = Instantiate(playerPrefab, new Vector3(-2.3f, 3.2f, 0), Quaternion.identity);

			//register with the camera
			FindObjectOfType<CameraFollow>().RegisterPlayer(player.transform);

			PlayerScript ps = player.GetComponent<PlayerScript>();

			//set up player information
			ps.swimForce = playerData.SwimSpeed * 20f;
			ps.maxInventoryCount = playerData.ItemCarryLimit;

			//again not good coding style
			ps.EnteredAir += FindObjectOfType<Show>().OnEnteredAir;
			ps.EnteredWater += FindObjectOfType<Show>().OnEnteredWater;

			//let the ship and distoration layer know when we enter and exit water
			ShipAndDistortion[] sad = FindObjectsOfType<ShipAndDistortion>();
			foreach (var item in sad)
			{
				item.RegisterPlayer(ps);
			}

			ps.Enter("air");

			//set up the survival mechanics
			ps.FoodEaten += FindObjectOfType<HungerMeter>().OnFoodEaten;
			FindObjectOfType<HungerMeter>().Starved += ps.OnStarved;

			//not good coding, but doing this to avoid messing with script execution order or setting
			//up a static messaging class
			OxygenMeter om = FindObjectOfType<OxygenMeter>();
			ps.EnteredAir += om.OnAirEntered;
			ps.EnteredWater += om.OnWaterEntered;
			om.OxygenDepleted += ps.OnOxygenDepleted;
			om.depleteSpeed = 1f / playerData.HoldBreath;

			//setup item detection
			player.GetComponentInChildren<InventoryTrigger>().ItemSelected += ps.OnItemSelected;
			player.GetComponentInChildren<InventoryTrigger>().ItemUnselected += ps.OnItemUnselected;


			FindObjectOfType<Inventory>().RegisterPlayer(ps);

			//set up crafting
			CraftingStation cs = FindObjectOfType<CraftingStation>();
			cs.foodBaseSpeed = playerData.FoodCookSpeedMultiplier;
			cs.shipBaseSpeed = playerData.ShipRoomCraftTimeMultiplier;
			cs.itemBaseSpeed = playerData.ItemCraftTimeMultiplier;

			DeathPanel dp = FindObjectOfType<PanelHolder>().deathPanel.GetComponent<DeathPanel>();
			dp.restart.onClick.AddListener(Restart);
			dp.mainMenu.onClick.AddListener(ReturnToMainMenu);
		}
	}


	public void Restart()
	{
		SceneManager.UnloadSceneAsync(2);
		SceneManager.LoadScene(2, LoadSceneMode.Additive);
	}

	public void ReturnToMainMenu()
	{

		SceneManager.LoadScene(1, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync(2);
	}
}
