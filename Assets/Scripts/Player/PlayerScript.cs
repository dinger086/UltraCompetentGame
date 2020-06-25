using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	public float footstepInterval = 0.5f;
	[FMODUnity.EventRef]
	public string footStep;
	private float lastFootstep;

	[FMODUnity.EventRef]
	public string waterEntry;

	[FMODUnity.EventRef]
	public string waterExit;

	private float jumpCooldown = 0f;
	private float jumpPause = 0.2f;
	public float jumpPower;

	private float swimCooldown = 0f;
	public float swimForce = 1f;

	public int maxInventoryCount;
	private List<ItemData> inventory = new List<ItemData>();
	private Item currentSelectedItem = null;

	private Rigidbody2D r2d;
    private bool onGround = false;
    private bool inWater = false;
	private bool isFacingRight = true;

	//for sending events to the oxygen meter
	public delegate void OxygenHandler();
	public event OxygenHandler EnteredAir;
	public event OxygenHandler EnteredWater;

	//for sending events to hunger meter
	public delegate void FoodHandler(float amt);
	public event FoodHandler FoodEaten;

	//for sending events to health meter
	public delegate void HealthHandler(float amt);
	public event HealthHandler Healed;
	public event HealthHandler Damaged;


	//for sending events to health meter
	//public delegate void InventoryHandler(List<ItemData> inventory);
	//public event InventoryHandler OpenInventory;
	//public event InventoryHandler CloseInventory;

	public delegate void InventoryHandler(ItemData item);
	public event InventoryHandler AddItem;



	//public event InventoryHandler CloseInventory;

    [SerializeField]
	//we should use this for making sure the player is inside the air bubble, we don't need an extra vector2
    BoxCollider2D PlayerShape;

    private void Awake()
    {
        r2d = gameObject.GetComponent<Rigidbody2D>();

		//Enter("air");
    }

	public void OnOxygenDepleted()
	{
		//we've run out of air
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

				//Hunter: I still think this looks better and it works much smoother with the current system
				r2d.AddForce(Vector2.up * 10f);

				break;
            case "water":
                inWater = true;
                r2d.drag = 2f;
                r2d.gravityScale = 0.1f;
				//onGround = true;

				FMODUnity.RuntimeManager.PlayOneShot(waterEntry);

				if (EnteredWater != null)
				{
					EnteredWater();
				}

                break;

        }
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
		Debug.Log("walking " + direction);
		r2d.AddForce(direction*20f, ForceMode2D.Force);
        

    }

	private void HandleDirection(bool right)
	{
		if (right)
		{
			if (!isFacingRight)
			{
				isFacingRight = true;
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
			}
		}
		else
		{
			if (isFacingRight)
			{
				isFacingRight = false;
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
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
		RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position - (Vector2.down/2f), Vector2.down, 1f,LayerMask.GetMask("Platform","Ground"));
		if (hit.transform !=null)
		{
			Debug.Log(hit.transform.tag);
			
			onGround = true;
			
		}
		else
		{
			onGround = false;
		}
		
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
			if (FoodEaten != null)
			{
				FoodEaten(10f);
			}
		}

		////for testing, not final
		//if (Input.GetKeyDown(KeyCode.I))
		//{

		//}

		//for testing, not final
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (inventory.Count < maxInventoryCount)
			{
				//we want to pick up an item
				if (currentSelectedItem != null)
				{
					
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
        switch (collision.gameObject.tag) {
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
					Debug.Log("entered air");
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
