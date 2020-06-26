using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidTimer : MonoBehaviour
{
	public float timeLimit = 300f;
	float currentTime = 0f;
	bool triggered = false;
	public GameObject squid;
	[FMODUnity.EventRef]
	public string kraken;
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
			FMODUnity.RuntimeManager.PlayOneShot(kraken);
			StartCoroutine(MoveSquid());
		}
    }

	IEnumerator MoveSquid()
	{
		while (true)
		{
			squid.transform.Translate(3f* Vector3.right * Time.deltaTime);
			yield return null;
		}
		
	}
}
