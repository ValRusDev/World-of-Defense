using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleProgressBar : MonoBehaviour
{
	public Image bar;
	public bool reverse;
	float maxValue;
	float value;

	public float currentValue
	{
		get
		{
			return value;
		}
	}

	public void SetDefault(float max)
	{
		maxValue = max;
		value = max;
		bar.fillAmount = 1;
	}

	public void SetSettings(float max, float current)
	{
		maxValue = max;
		value = current;
		if (reverse)
			bar.fillAmount = 1 - (current / max);
		else
			bar.fillAmount = current / max;
	}

	public void AdjustCurrentValue(float adjust)
	{
		value += adjust;
		if (value < 0)
			value = 0;
		if (value > maxValue)
			value = maxValue;

		bar.fillAmount = value / maxValue;
	}
}
