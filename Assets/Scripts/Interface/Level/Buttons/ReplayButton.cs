using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour
{
	Button button;
	LevelManager levelManager;

	void Awake()
	{
		button = GetComponent<Button>();
	}

	void Start()
	{
		levelManager = LevelManager.Instance;
		button.onClick.AddListener(levelManager.ReloadLevel);
	}
}
