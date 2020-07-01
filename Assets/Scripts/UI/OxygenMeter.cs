using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenMeter : MonoBehaviour
{
	[SerializeField]
	Image meter;
	[SerializeField]
	RectTransform fx;
	//default is set to 30 seconds
	public float depleteSpeed = 0.000001f;
	float fillSpeed = 3f;

	public delegate void OxygenDepletionHandler();
	public event OxygenDepletionHandler OxygenDepleted;

	public void OnWaterEntered()
	{
		Debug.Log("water");
		StopAllCoroutines();
		StartCoroutine(Change(true));
	}

	public void OnAirEntered()
	{
		Debug.Log("air");
		StopAllCoroutines();
		StartCoroutine(Change(false));
	}

	IEnumerator Change(bool decrease)
	{
		if (decrease)
		{
			while (meter.fillAmount > 0f)
			{
				meter.fillAmount -= Time.deltaTime * depleteSpeed;
				AdjustFX();
				yield return null;
			}
		}
		else
		{
			while (meter.fillAmount < 100f)
			{
				meter.fillAmount += Time.deltaTime * fillSpeed;
				AdjustFX();
				yield return null;
			}
		}

		if (meter.fillAmount <= 0)
		{
			if (OxygenDepleted != null)
			{
				OxygenDepleted();
			}
		}
	}

	void AdjustFX()
	{
		fx.anchoredPosition = new Vector2(0, Mathf.Lerp(-200f, 200f, meter.fillAmount));
	}
}
