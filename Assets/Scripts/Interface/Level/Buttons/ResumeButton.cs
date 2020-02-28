using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
	public GameObject pauseCanvas;
	Button button;
	LevelManager levelManager;

	void Awake()
	{
		button = GetComponent<Button>();
	}

	void Start()
	{
		levelManager = LevelManager.Instance;
		button.onClick.AddListener(() => levelManager.GameUnpause(pauseCanvas));
	}
}
