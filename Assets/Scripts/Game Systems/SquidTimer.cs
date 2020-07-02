using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidTimer : MonoBehaviour
{
	public float timeLimit = 300f;
	float currentTime = 0f;

	bool triggered = false;
	bool deathPointReached = false;

	public GameObject squid;
	[FMODUnity.EventRef]
	public string kraken;

	public delegate void KrakenHandler();
	public event KrakenHandler KrakenAppeared;
	public event KrakenHandler ReachedDeathPoint;
    // Start is called before the first frame update
    void Start()
    {
		//StartCoroutine(MoveSquid());
	}

    // Update is called once per frame
    void Update()
    {
		currentTime += Time.deltaTime;

		if (! triggered && currentTime >= timeLimit)
		{
			triggered = true;
			squid.SetActive(true);
			if (KrakenAppeared != null)
			{
				KrakenAppeared();
			}

			FMODUnity.RuntimeManager.PlayOneShot(kraken);
			StartCoroutine(MoveSquid());
		}
    }

	IEnumerator MoveSquid()
	{
		while (true)
		{
			squid.transform.Translate(3f* Vector3.right * Time.deltaTime);

			if (squid.transform.position.x > 7f && !deathPointReached)
			{
				deathPointReached = true;
				if (ReachedDeathPoint != null)
				{
					ReachedDeathPoint();
				}
			}
			yield return null;
		}
		
	}
}
