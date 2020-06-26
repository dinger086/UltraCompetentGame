using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data")]
public class PlayerData : BasePlayerData
{
	public string description;

	public GameObject prefab;
	public Sprite image;

	public float SwimSpeed
	{
		get { return swimSpeed; }
	}

	public float HoldBreath
	{
		get { return holdBreath; }
	}

	public float ItemCraftTimeMultiplier
	{
		get { return itemCraftTimeMultiplier; }
	}

	public float ItemIngredientMultiplier
	{
		get { return itemIngredientMultiplier; }
	}

	public float ItemSpawnRate
	{
		get { return itemSpawnRate; }
	}

	public int ItemCarryLimit
	{
		get { return itemCarryLimit; }
	}

	public float FoodCookSpeedMultiplier
	{
		get { return foodCookTimeMultiplier; }
	}

	public float BaseHealth
	{
		get { return baseHealth; }
	}

	public float HealSpeed
	{
		get { return healSpeed; }
	}

	public float ShipRoomCraftTimeMultiplier
	{
		get { return shipRoomCraftTimeMultiplier ; }
	}

	public float SightDistance
	{
		get { return sightDistance; }
	}
}
