using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	public GameObject[] itemPrefab;
	public float interval;
	int count = 0;
	int max = 30;

	// Start is called before the first frame update
	void Start()
    {
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
				Vector2 pos = new Vector2(Random.Range(-20,20),20);

				Collider2D col = Physics2D.OverlapBox(pos, Vector2.one, 0);
				if (col != null)
				{
					//we are trying to go inside the ship, so pick a different point
					while (col.tag == "AirBubble" || col.tag == "Ground" || col.tag == "Platform")
					{

						pos = new Vector2(Random.Range(-60, 60), 20);
						//without this it causes a potentially infinite loop, of course
						col = Physics2D.OverlapBox(pos, Vector2.one, 0);

						//avoids null errors
						if (col == null)
						{
							break;
						}
					}
				}

				count++;
				//Debug.Log(count);
				SimplePool.Spawn(itemPrefab[Random.Range(0,itemPrefab.Length)], pos, Quaternion.identity);
			}


			yield return new WaitForSeconds(interval);
		}

	}
}
