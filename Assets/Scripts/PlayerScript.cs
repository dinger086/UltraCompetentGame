using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
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

    //for making shure the entire player body is inside an air buble, will probably not work too well with konvex shapes as im just testing the 4 corners 1
    private Vector2 size;
    [SerializeField]
    BoxCollider2D OnGroundTrigger;
    [SerializeField]
	//we should use this for making sure the player is inside the air bubble, we don't need an extra vector2
    BoxCollider2D PlayerShape;

    private void Awake()
    {
        r2d = gameObject.GetComponent<Rigidbody2D>();

		//not good coding, but doing this to avoid messing with script execution order or setting
		//up a static messaging class
		EnteredAir += FindObjectOfType<OxygenMeter>().OnAirEntered;
		EnteredWater += FindObjectOfType<OxygenMeter>().OnWaterEntered;
        size = PlayerShape.size;

		GetComponentInChildren<InventoryTrigger>().ItemSelected += OnItemSelected;
		GetComponentInChildren<InventoryTrigger>().ItemUnselected += OnItemUnselected;

		Enter("water");
    }

	private void OnItemUnselected(Item i)
	{
		currentSelectedItem = null;
	}

	private void OnItemSelected(Item i)
	{
		Debug.Log(i.itemData.name);
		currentSelectedItem = i;
	}

	public void Enter(string key)
    {
        switch (key)
        { 
            case "air":
                inWater = false;
                r2d.drag = 0.4f;
                r2d.gravityScale = 1f;

				if (EnteredAir != null)
				{
					EnteredAir();
				}

				break;
            case "water":
                inWater = true;
                r2d.drag = 2f;
                r2d.gravityScale = 0.1f;
                onGround = true;
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
			if (isFacingRight)
			{
				isFacingRight = false;
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
			}
		}
        if (Input.GetKey("d"))
        {
            direction.x += 1f;
			//switch the direction of the character if necessary
			if (!isFacingRight)
			{
				isFacingRight = true;
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
			}
		}

        direction.Normalize();
        direction *= swimForce*delta;
        r2d.AddForce(direction, ForceMode2D.Force);

    }

    private void Walk()
    {
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
			if (isFacingRight)
			{
				isFacingRight = false;
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x,180f, transform.eulerAngles.z);
			}
		}
		if (Input.GetKey("d"))
		{
			direction.x += 1f;
			//switch the direction of the character if necessary
			if (!isFacingRight)
			{
				isFacingRight = true;
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
			}
		}

		direction *= delta;
        
        r2d.AddForce(direction*20f, ForceMode2D.Force);
        

    }

    private float jumpCooldown = 0f;
    private float jumpPause = 0.2f;

    private void Update()
    {
        jumpCooldown -= Time.deltaTime;

        switch (inWater)
		{
            case true:
                Swim();
                break;
            case false:
                Walk();
                break;
        }

		//for testing, not final
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (FoodEaten != null)
			{
				FoodEaten(10f);
			}
		}

		//for testing, not final
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (inventory.Count < maxInventoryCount)
			{
				//we want to pick up an item
				if (currentSelectedItem != null)
				{
					
					inventory.Add(currentSelectedItem.itemData);
					Debug.Log("Adding Item");
					Debug.Log(inventory.Count);
					Debug.Log(inventory[inventory.Count - 1].name);

					//if we do this before, we get errors!
					currentSelectedItem.gameObject.SetActive(false);
					//set the currentselectedItem to null
					currentSelectedItem = null;
				}
			}
			
		}
	}

    public float jumpPower;
    private void Jump()
    {
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
                
                Vector3 worldPos = transform.TransformPoint(PlayerShape.offset);
                
                float top = worldPos.y + (size.y / 2f);
                float btm = worldPos.y - (size.y / 2f);
                float left = worldPos.x - (size.x / 2f);
                float right = worldPos.x + (size.x / 2f);

                Vector2 topLeft = new Vector3(left, top);
                Vector2 topRight = new Vector3(right, top);
                Vector2 btmLeft = new Vector3(left, btm);
                Vector2 btmRight = new Vector3(right, btm);

				//we can do it this way
				//Vector2 topLeft = new Vector3(PlayerShape.bounds.min.x, PlayerShape.bounds.max.y);
				//Vector2 topRight = new Vector3(PlayerShape.bounds.max.x, PlayerShape.bounds.max.y);
				//Vector2 bottomLeft = new Vector3(PlayerShape.bounds.min.x, PlayerShape.bounds.min.y);
				//Vector2 bottomRight = new Vector3(PlayerShape.bounds.max.x, PlayerShape.bounds.min.y);


				if (collision.OverlapPoint(topLeft)&& 
                    collision.OverlapPoint(topRight)&& 
                    collision.OverlapPoint(btmLeft)&&
                    collision.OverlapPoint(btmRight)&&inWater)
                {
                    Enter("air");
                }
                
                break;
            case "Platform":
                if (!inWater&&collision.IsTouching(OnGroundTrigger))
                {
                    onGround = true;
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
