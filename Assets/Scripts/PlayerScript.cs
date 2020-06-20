using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D r2d;
    private bool onGround = false;
    private bool inWater = false;

    
    private void Awake()
    {
        r2d = gameObject.GetComponent<Rigidbody2D>();
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
                break;
            case "water":
                inWater = true;
                r2d.drag = 2f;
                r2d.gravityScale = 0.1f;
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

    }
    private void Update()
    {
        switch (inWater) {
            case true:
                Swim();
                break;
            case false:
                Walk();
                break;
        }
    }

    private ContactFilter2D con = new ContactFilter2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AirBubble")
        {
            EdgeCollider2D edge = collision.gameObject.GetComponent<EdgeCollider2D>();
            Collider2D test = new Collider2D();
            int a = collision.OverlapCollider(con.NoFilter, test);

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
