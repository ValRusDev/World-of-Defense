using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCountLabel : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;

	WavesManager wavesManager;
	TextMeshProUGUI textMeshPro;

	void Awake()
	{
		ownTransform = transform;
		textMeshPro = GetComponent<TextMeshProUGUI>();
	}

	void Start()
	{
		wavesManager = WavesManager.Instance;
		wavesManager.OnWaveNumberChange += WaveLabelChange;

		WaveLabelChange(wavesManager.WavesState);
	}

	void WaveLabelChange(string value)
	{
		textMeshPro.text = value;
	}
}
