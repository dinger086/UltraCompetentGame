using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	public Animator anim;
	public float footstepInterval = 0.5f;
	[FMODUnity.EventRef]
	public string footStep;
	private float lastFootstep;

	[FMODUnity.EventRef]
	public string waterEntry;

	[FMODUnity.EventRef]
	public string waterExit;

	[FMODUnity.EventRef]
	public string waterAmbient;
	FMOD.Studio.EventInstance water;

	[FMODUnity.EventRef]
	public string thump;

	[FMODUnity.EventRef]
	public string pickup;

	private float jumpCooldown = 0f;
	private float jumpPause = 0.2f;
	public float jumpPower;

	private float swimCooldown = 0f;
	public float swimForce = 1f;

	public int maxInventoryCount;
	private List<ItemData> inventory = new List<ItemData>();
	private Item currentSelectedItem = null;
	private GameObject currentselectedAnimal = null;

	private Rigidbody2D r2d;
    private bool onGround = false;
    private bool inWater = false;
	private bool isFacingRight = false;



	//for sending events to the oxygen meter
	public delegate void OxygenHandler();
	public event OxygenHandler EnteredAir;
	public event OxygenHandler EnteredWater;

	//for sending events to hunger meter
	public delegate void FoodHandler(float amt);
	public event FoodHandler FoodEaten;

	//for sending events to health meter
	public delegate void HealthHandler();
	public event HealthHandler Healed;
	public event HealthHandler Damaged;
	public event HealthHandler Deplete;


	//for sending events to health meter
	//public delegate void InventoryHandler(List<ItemData> inventory);
	//public event InventoryHandler OpenInventory;
	//public event InventoryHandler CloseInventory;

	public delegate void InventoryHandler(ItemData item);
	public event InventoryHandler AddItem;
	public event InventoryHandler RemoveItem;



	//public event InventoryHandler CloseInventory;

	[SerializeField]
	//we should use this for making sure the player is inside the air bubble, we don't need an extra vector2
    BoxCollider2D PlayerShape;


	public void OnItemRemoved(ItemData item)
	{
		inventory.Remove(item);
	}

	private void Awake()
    {
        r2d = gameObject.GetComponent<Rigidbody2D>();

		//Enter("air");
    }

	internal void OnVictoryAchieved()
	{
		enabled = false;
	}

	public void OnOxygenDepleted()
	{
		//we've run out of air
		if (Deplete != null)
		{
			Deplete();
		}

	}

	public void OnItemUnselected(Item i)
	{
		currentSelectedItem = null;
	}

	public void OnItemSelected(Item i)
	{
		//Debug.Log(i.itemData.name);
		currentSelectedItem = i;
	}

	public void OnStarved()
	{
		//we're dead from starvation
		if (Deplete != null)
		{
			Deplete();
		}
	}


	public void Enter(string key)
    {
        switch (key)
        { 
            case "air":
                inWater = false;
                r2d.drag = 0.4f;
                r2d.gravityScale = 1f;

				FMODUnity.RuntimeManager.PlayOneShot(waterExit);

				if (EnteredAir != null)
				{
					EnteredAir();
				}

				anim.SetBool("InWater", false);
				FMOD.Studio.PLAYBACK_STATE state;
				water.getPlaybackState(out state);
				if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
				{
					water.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				}

				//Hunter: I still think this looks better and it works much smoother with the current system
				r2d.AddForce(Vector2.up * 10f);

				break;
            case "water":
                inWater = true;
				anim.SetBool("InWater", true);
				r2d.drag = 2f;
                r2d.gravityScale = 0.1f;
				//onGround = true;

				FMODUnity.RuntimeManager.PlayOneShot(waterEntry);

				if (EnteredWater != null)
				{
					EnteredWater();
				}


				water.getPlaybackState(out state);
				if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
				{
					water = FMODUnity.RuntimeManager.CreateInstance(waterAmbient);
					water.start();
				}
                break;

        }
    }

	internal void OnDied()
	{
		enabled = false;
		anim.enabled = false;
	}

	public void OnAnimalUnselected(GameObject go)
	{
		currentselectedAnimal = null;
	}

	public void OnAnimalSelected(GameObject go)
	{
		currentselectedAnimal = go;
	}

	private void Swim()
    {
        float delta = Time.deltaTime;
        Vector2 direction = new Vector2(0, 0);
        if (Input.GetKey("w"))
        {
            direction.y += 1f;
        }
        if (Input.GetKey("s"))
        {
            direction.y -= 1f;
        }
        if (Input.GetKey("a"))
        {
            direction.x -= 1f;
			//switch the direction of the character if necessary
			HandleDirection(false);
		}
        if (Input.GetKey("d"))
        {
            direction.x += 1f;
			//switch the direction of the character if necessary
			HandleDirection(true);
		}

        direction.Normalize();
        direction *= swimForce*delta;

		//Debug.Log(direction);
		anim.SetBool("Moving", direction.sqrMagnitude > 0f);
		r2d.AddForce(direction, ForceMode2D.Force);

    }

    private void Walk()
    {
		//Debug.Log("walking");
        float delta = Time.deltaTime;

		Vector2 direction = new Vector2(0, 0);
        if (Input.GetKey("w"))
        {
            Jump();
        }
        if (Input.GetKey("s"))
		{
			//direction.y -= 1f;
		}
		if (Input.GetKey("a"))
		{
		    direction.x -= 1f;
			//switch the direction of the character if necessary
			HandleDirection(false);
			HandleFootstep();
			
		}
		if (Input.GetKey("d"))
		{
			direction.x += 1f;
			//switch the direction of the character if necessary
			HandleDirection(true);

			HandleFootstep();
		}

		direction *= delta;
		//Debug.Log("walking " + direction);
		anim.SetBool("Moving", direction.sqrMagnitude > 0f);
		r2d.AddForce(direction*20f, ForceMode2D.Force);
        

    }

	private void HandleDirection(bool right)
	{
		if (right)
		{
			if (!isFacingRight)
			{
				isFacingRight = true;
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
			}
		}
		else
		{
			if (isFacingRight)
			{
				isFacingRight = false;
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
			}
		}
	}

	private void HandleFootstep()
	{
		if (Time.time - lastFootstep >= footstepInterval)
		{
			//Debug.Log("here");
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, .5f);
			if (hit.transform != null)
			{
				//Debug.Log(hit.transform.tag);
				if (hit.transform.tag == "Platform")
				{
					FMOD.Studio.EventInstance fs = FMODUnity.RuntimeManager.CreateInstance(footStep);
					if (inWater)
					{
						fs.setParameterByName("Water", 1f);
						fs.setParameterByName("Metal", 1f);
						fs.start();
					}
					else
					{
						//Debug.Log("playing sound");
						fs.setParameterByName("Water", 0f);
						fs.setParameterByName("Metal", 1f);
						fs.start();
					}
				}
				else if (hit.transform.tag == "Ground")
				{
					FMOD.Studio.EventInstance fs = FMODUnity.RuntimeManager.CreateInstance(footStep);
					if (inWater)
					{
						fs.setParameterByName("Water", 1f);
						fs.setParameterByName("Regular", 1f);
						fs.start();
					}
					else
					{
						fs.setParameterByName("Water", 0f);
						fs.setParameterByName("Regular", 1f);
						fs.start();
					}
				}
				lastFootstep = Time.time;
			}
			
			
		}
	}

	private void IsGrounded()
	{
		RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position - (Vector2.down), Vector2.down, 2f,LayerMask.GetMask("Platform","Ground"));
		if (hit.transform !=null)
		{
			//Debug.Log(hit.transform.tag);

			if (!onGround)
			{
				onGround = true;
				FMODUnity.RuntimeManager.PlayOneShot(thump);
			}
			
			
		}
		else
		{
			onGround = false;
		}

		anim.SetBool("OnGround", onGround);
		
	}

    private void Update()
    {
        jumpCooldown -= Time.deltaTime;


		IsGrounded();

		//Debug.Log(onGround);

		if (inWater)
		{
			if (!onGround)
			{
				Swim();
			}
			else
			{
				Walk();
			}
		}
		else
		{
			Walk();
		}

		//for testing, not final
		if (Input.GetKeyDown(KeyCode.F))
		{
			for (int i = 0; i < inventory.Count; i++)
			{
				if (inventory[i].name == "Cooked Fish")
				{
					if (RemoveItem != null)
					{
						RemoveItem(inventory[i]);
					}

					inventory.RemoveAt(i);
					if (FoodEaten != null)
					{
						FoodEaten(10f);
					}

					if (Healed != null)
					{
						Healed();
					}

					break;
				}
			}
			
		}

		////for testing, not final
		//if (Input.GetKeyDown(KeyCode.I))
		//{

		//}

		//for testing, not final
		if (Input.GetKeyDown(KeyCode.E))
		{
			//Debug.Log("e");
			if (currentSelectedItem != null)
			{
				if (inventory.Count < maxInventoryCount)
				{

					//Debug.Log("less");
					//we want to pick up an item
					FMODUnity.RuntimeManager.PlayOneShot(pickup);
					//Debug.Log("not null");
					inventory.Add(currentSelectedItem.itemData);
					//Debug.Log("Adding Item");
					//Debug.Log(inventory.Count);
					//Debug.Log(inventory[inventory.Count - 1].name);

					if (AddItem != null)
					{
						AddItem(currentSelectedItem.itemData);
					}

					//if we do this before, we get errors!
					currentSelectedItem.gameObject.SetActive(false);
					//set the currentselectedItem to null
					currentSelectedItem = null;
				}
				
			} //we are near a fish
			else if (currentselectedAnimal != null)
			{
				currentselectedAnimal.GetComponent<Animal>().Die();
				currentselectedAnimal = null;
			}
			
		}
	}

 

    private void Jump()
    {
		//Debug.Log("jump");
        if (jumpCooldown <= 0 &&onGround)
        {
            r2d.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            jumpCooldown = jumpPause;
            onGround = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
		{
            case "AirBubble":



				//we can do it this way
				Vector2 topLeft = new Vector3(PlayerShape.bounds.min.x, PlayerShape.bounds.max.y);
				Vector2 topRight = new Vector3(PlayerShape.bounds.max.x, PlayerShape.bounds.max.y);
				Vector2 bottomLeft = new Vector3(PlayerShape.bounds.min.x, PlayerShape.bounds.min.y);
				Vector2 bottomRight = new Vector3(PlayerShape.bounds.max.x, PlayerShape.bounds.min.y);


				if (collision.OverlapPoint(topLeft)&& 
                    collision.OverlapPoint(topRight)&& 
                    collision.OverlapPoint(bottomLeft) &&
                    collision.OverlapPoint(bottomRight) && inWater)
                {
					//Debug.Log("entered air");
                    Enter("air");
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AirBubble")
        {
            Enter("water");
        }
    }
}
