using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeCountLabel : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;

	LevelManager levelManager;
	TextMeshProUGUI textMeshPro;

	void Awake()
	{
		ownTransform = transform;
		textMeshPro = GetComponent<TextMeshProUGUI>();
	}

	void Start()
	{
		levelManager = LevelManager.Instance;
		textMeshPro.text = levelManager.startLivesCount.ToString();

		levelManager.OnLivesCountChange += LifeLabelChange;
	}

	void LifeLabelChange()
	{
		string decodingLife = B64X.Decode(levelManager.currentLivesCount);
		textMeshPro.text = decodingLife;
	}
}
