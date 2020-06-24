using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
	public GameObject animalPrefab;
	public float interval;
	int count = 0;
	int max = 30;

    // Start is called before the first frame update
    void Start()
    {
		//SimplePool.
		StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	IEnumerator Spawn()
	{
		while (true)
		{
			if (count < max)
			{
				Vector2 pos = Random.insideUnitCircle * 10f;

				Collider2D col = Physics2D.OverlapBox(pos, Vector2.one, 0);
				//we are trying to go inside the ship, so pick a different point
					while (col != null && (col.tag == "AirBubble" || col.tag == "Ground" || col.tag == "Platform"))
					{

					Debug.Log("stuck in spawner");
						pos = Random.insideUnitCircle * 10f;
						//without this it causes a potentially infinite loop, of course
						col = Physics2D.OverlapBox(pos, Vector2.one, 0);

						//avoids null errors
						if (col == null)
						{
							break;
						}
					}

				count++;
				Debug.Log(count);
				(SimplePool.Spawn(animalPrefab, pos, Quaternion.identity)).GetComponent<Animal>().AnimalDied += OnAnimalKilled;
			}
			

			yield return new WaitForSeconds(interval);
		}
		
	}

	void OnAnimalKilled(Animal a)
	{
		count--;
		//to avoid resubscribing and receiving double or triple event calls
		a.AnimalDied -= OnAnimalKilled;
	}
}
