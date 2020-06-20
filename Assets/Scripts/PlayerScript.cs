using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D r2d;
    private bool onGround = false;
    private bool inWater = false;

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

	private void Awake()
    {
        r2d = gameObject.GetComponent<Rigidbody2D>();

		//not good coding, but doing this to avoid messing with script execution order or setting
		//up a static messaging class
		EnteredAir += FindObjectOfType<OxygenMeter>().OnAirEntered;
		EnteredWater += FindObjectOfType<OxygenMeter>().OnWaterEntered;

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
		//added for testing, not final

		//if (Input.GetKey("w"))
		//{
		//	direction.y += 1f;
		//}
		//if (Input.GetKey("s"))
		//{
		//	direction.y -= 1f;
		//}
		//if (Input.GetKey("a"))
		//{
		//	direction.x -= 1f;
		//}
		//if (Input.GetKey("d"))
		//{
		//	direction.x += 1f;
		//}

		//direction.Normalize();
		//direction *= swimForce * delta;
		//r2d.AddForce(direction, ForceMode2D.Force);
	}

    private void Update()
    {
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

    private ContactFilter2D con = new ContactFilter2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AirBubble")
        {
			r2d.AddForce((collision.transform.position - transform.position).normalized * 25f);
            Enter("air");
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
