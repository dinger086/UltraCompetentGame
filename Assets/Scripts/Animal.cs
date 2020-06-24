using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	private Rigidbody2D r2d;
	private float speed = 2f;
	private bool isFacingRight = false;

	private float attackDelay = 3f;

	private float lifeTime;

	private float health;

	private float expectedTravelTime;
	private float currentTravelTime;

	public delegate void AnimalDeathHandler(Animal a);
	public event AnimalDeathHandler AnimalDied;

	List<Transform> neighbors = new List<Transform>();

	private enum AnimalState
	{
		Wander,
		Attack,
		Flee
	}

	private AnimalState state;
	Transform targetObject;
	Vector3 targetPosition;

	private void OnEnable()
	{
		neighbors = new List<Transform>();
		r2d = gameObject.GetComponent<Rigidbody2D>();
		state = AnimalState.Wander;
	}

	// Start is called before the first frame update
	private void Start()
    {
        
    }

	// Update is called once per frame
	private void Update()
    {
		switch (state)
		{
			case AnimalState.Wander:
				Wander();
				break;
			case AnimalState.Attack:
				Attack();
				break;
			case AnimalState.Flee:
				Flee();
				break;
			default:
				break;
		}
	}

	public void ReceiveDamage(float amt)
	{
		//we're dead
		if (health-amt <= 0)
		{
			if (AnimalDied != null)
			{
				AnimalDied(this);
			}
		}
	}

	private void Flee()
	{

	}

	private void Wander()
	{
		if (Vector3.Distance(transform.position, targetPosition) > 0.2f && currentTravelTime < expectedTravelTime)
		{
			Vector2 direction = targetPosition - transform.position;
			direction.Normalize();
			direction *= speed * Time.deltaTime;
			r2d.AddForce(direction, ForceMode2D.Force);
			HandleDirection(direction.x > 0f);
			currentTravelTime += Time.deltaTime;
		}
		else
		{
			Vector2 r = Random.insideUnitCircle * 10f;
			Vector3 rand = new Vector3(r.x, r.y, 0f);
			targetPosition = transform.position + rand;
			//Debug.Log(targetPosition);
			Collider2D col = Physics2D.OverlapBox(targetPosition, Vector2.one, 0);
			if (col != null)
			{
				//we are trying to go inside the ship, so pick a different point
				while (col.tag == "AirBubble" || col.tag == "Ground" || col.tag == "Platform")
				{
					Debug.Log("stuck in animal collision");
					r = Random.insideUnitCircle * 10f;
					rand = new Vector3(r.x, r.y, 0f);
					targetPosition = transform.position + rand;
					//without this it causes a potentially infinite loop, of course
					col = Physics2D.OverlapBox(targetPosition, Vector2.one, 0);

					//avoids null errors
					if (col == null)
					{
						break;
					}
				}

				Vector2 direction = targetPosition - transform.position;
				float distance = direction.magnitude;
				direction.Normalize();

				//we also don't want to deal with 2D pathfinding, so just avoid traveling through ground, platform, etc
				RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one, 0f, direction, distance);
				if (hit.transform != null)
				{
					while (hit.transform.tag == "AirBubble" || hit.transform.tag == "Ground" || hit.transform.tag == "Platform")
					{
						Debug.Log("stuck in animal ray cast");
						r = Random.insideUnitCircle * 10f;
						rand = new Vector3(r.x, r.y, 0f);
						targetPosition = transform.position + rand;

						//we need to reset the direction and the distance in order to raycast again
						direction = targetPosition - transform.position;
						distance = direction.magnitude;
						hit = Physics2D.BoxCast(transform.position, Vector2.one, 0f, direction, distance);
						
						//gets out of potentially infinite loop
						if (hit.transform == null)
						{
							break;
						}
					}
				}

			}
			// we should finally have a valid position
			expectedTravelTime = Vector2.Distance(transform.position, targetPosition) / speed;
			currentTravelTime = 0f;
		}
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
				transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
			}
		}
	}

	private void Attack()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Monster")
		{
			state = AnimalState.Flee;
			targetObject = collision.transform;
		}
	}



}
