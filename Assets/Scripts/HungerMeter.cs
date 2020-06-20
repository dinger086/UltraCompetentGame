using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerMeter : MonoBehaviour
{
	Image meter;
	RectTransform fx;
	float depleteSpeed = 0.01f;
	float fillSpeed = 1f;
	float eatingDelay = 5f;

	Coroutine depletion;

	private void Awake()
	{
		meter = GetComponent<Image>();
		//again, not good coding, but avoids strange behavior when GetComponentInChildren returns the
		// RectTransform of this object, not the child
		fx = transform.GetChild(0).GetComponent<RectTransform>();

		//not good coding, fix later
		FindObjectOfType<PlayerScript>().FoodEaten += OnFoodEaten;

		//start the depletion
		depletion = StartCoroutine(Deplete());
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//private void OnWaterEntered()
	//{
	//	Debug.Log("water");
		
	//}

	private void OnFoodEaten(float amt)
	{
		Debug.Log("food");
		StartCoroutine(Fill(amt));
	}

	IEnumerator Deplete()
	{
			while (meter.fillAmount > 0f)
			{
				meter.fillAmount -= Time.deltaTime * depleteSpeed;
				AdjustFX();
				yield return null;
			}
	}

	IEnumerator Fill(float amt)
	{
		// first pause the depletion
		StopCoroutine(depletion);

		//remap the amount to between 0 and 1
		amt = amt / 100f;

		float t = meter.fillAmount + amt;

		if (t > 1f)
		{
			t = 1f;
		}

		float amtAdded = 0;
		while (amtAdded < amt)
		{
			amtAdded += Time.deltaTime * fillSpeed;
			meter.fillAmount += Time.deltaTime * fillSpeed;
			AdjustFX();
			Debug.Log("running");
			yield return null;
		}

		Debug.Log("finished eating food");

		//give us a 5 second delay before starting to drain again
		Invoke("Depletion", eatingDelay);
	}

	void Depletion()
	{
		depletion = StartCoroutine(Deplete());
	}

	void AdjustFX()
	{
		//these are magic numbers and will have to change!
		fx.anchoredPosition = new Vector2(0, Mathf.Lerp(-170f, 170f, meter.fillAmount));
	}
}
