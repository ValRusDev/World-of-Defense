using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldCountLabel : MonoBehaviour
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
		textMeshPro.text = levelManager.startGold.ToString();

		levelManager.OnGoldChange += GoldLabelChange;
	}

	void GoldLabelChange()
	{
		string decodingGold = B64X.Decode(levelManager.currentGold);
		textMeshPro.text = decodingGold;
	}
}
