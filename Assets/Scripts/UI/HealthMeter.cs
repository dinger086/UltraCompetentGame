using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
	[SerializeField]
	Image meter;
	[SerializeField]
	RectTransform fx;
	float depleteSpeed = 0.1f;
	float fillSpeed = 1f;

	Coroutine depletion;


	IEnumerator Deplete()
	{
		while (meter.fillAmount > 0f)
		{
			meter.fillAmount -= Time.deltaTime * depleteSpeed;
			AdjustFX();
			yield return null;
		}

		if (meter.fillAmount <= 0)
		{
			//if (Starved != null)
			//{
			//	Starved();
			//}
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
	}

	public void Depletion()
	{
		depletion = StartCoroutine(Deplete());
	}

	public void Fill()
	{
		StartCoroutine(Fill(100f));
	}

	void AdjustFX()
	{
		//these are magic numbers and will have to change!
		fx.anchoredPosition = new Vector2(0, Mathf.Lerp(-200f, 200f, meter.fillAmount));
	}
}
