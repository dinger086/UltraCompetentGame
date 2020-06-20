using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	private Rigidbody2D r2d;
	private float speed = 2f;

	private enum AnimalState
	{
		Wander,
		Attack,
		Flee
	}

	private AnimalState state;
	Transform targetObject;
	Vector3 targetPosition;

	private void Awake()
	{
		r2d = gameObject.GetComponent<Rigidbody2D>();
		state = AnimalState.Wander;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

	void Flee()
	{

	}

	void Wander()
	{
		if (Vector3.Distance(transform.position, targetPosition) > 0.2f)
		{
			Vector2 direction = targetPosition - transform.position;
			direction.Normalize();
			direction *= speed * Time.deltaTime;
			r2d.AddForce(direction, ForceMode2D.Force);
		}
		else
		{
			targetPosition = Random.insideUnitCircle * 10f;
		}
	}

	void Attack()
	{

	}
}
