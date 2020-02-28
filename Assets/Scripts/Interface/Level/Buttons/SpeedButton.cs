using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
	public Sprite normalSpeed;
	public Sprite fastSpeed;

	Image image;
	Button button;
	LevelManager levelManager;

	float currentSpeed;

	void Awake()
	{
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		button.onClick.AddListener(ChangeSpeed);

		currentSpeed = Time.timeScale;
	}

	void Start()
    {
		levelManager = LevelManager.Instance;
    }

	void ChangeSpeed()
	{
		if (currentSpeed == 1.0f)
		{
			image.sprite = normalSpeed;
			currentSpeed = 2.0f;
		}
		else
		{
			image.sprite = fastSpeed;
			currentSpeed = 1.0f;
		}

		levelManager.SetGameSpeed(currentSpeed);
	}
}
