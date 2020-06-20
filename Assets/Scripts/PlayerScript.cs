using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D r2d;
    public bool onGround = false;
    public bool inWater = false;

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
    BoxCollider2D PlayerShape;

    private void Awake()
    {
        r2d = gameObject.GetComponent<Rigidbody2D>();

		//not good coding, but doing this to avoid messing with script execution order or setting
		//up a static messaging class
		EnteredAir += FindObjectOfType<OxygenMeter>().OnAirEntered;
		EnteredWater += FindObjectOfType<OxygenMeter>().OnWaterEntered;
        size = PlayerShape.size;
		Enter("water");
    }

    public void Enter(string key)
    {
        switch (key)
        { 
            case "air":
                inWater = false;
                r2d.drag = 0.1f;
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

    private float swimCooldown = 0f;
    public float swimForce = 1f;
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
        }
        if (Input.GetKey("d"))
        {
            direction.x += 1f;
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
		}
		if (Input.GetKey("d"))
		{
			direction.x += 1f;
		}

		direction *= swimForce * delta;
		r2d.AddForce(direction, ForceMode2D.Force);
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
