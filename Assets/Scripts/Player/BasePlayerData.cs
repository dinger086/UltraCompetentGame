using UnityEngine;

// we should probably set the base stats here,
//then inherit from this class for the
//individual player types
[CreateAssetMenu(fileName = "Base Player Data")]
public class BasePlayerData : ScriptableObject
{
	[SerializeField]
	protected float swimSpeed;
	[SerializeField]
	protected float holdBreath;
	[SerializeField]
	protected float itemCraftTimeMultiplier;
	[SerializeField]
	protected float itemIngredientMultiplier;
	[SerializeField]
	protected float itemSpawnRate;
	[SerializeField]
	protected int itemCarryLimit;
	[SerializeField]
	protected float foodCookTimeMultiplier;
	[SerializeField]
	protected float baseHealth;
	[SerializeField]
	protected float healSpeed;
	[SerializeField]
	protected float shipRoomCraftTimeMultiplier;
	[SerializeField]
	protected float sightDistance;
}
