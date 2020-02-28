using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartWaveButton : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;

	/// <summary>
	/// Объект, к которому прицеплена кнопка
	/// </summary>
	public Transform target;

	Camera mainCamera;

	/// <summary>
	/// Круговой индикатор
	/// </summary>
	[Header("Круговой индикатор"), Tooltip("Круговой индикатор")]
	public CircleProgressBar circleBar;
	float circleBarMaxValue;
	float currentCircleBarValue;

	StartWaveButtonCanvas startWaveButtonCanvas =>
		transform.parent.GetComponent<StartWaveButtonCanvas>();

	WavesManager wavesManager;

	Wave Wave =>
		startWaveButtonCanvas.Wave;

	Button button;

	SpriteRenderer buttonSpriteRenderer;
	Renderer buttonRenderer;
	RectTransform buttonRectTransform;
	float buttonWidth;
	float buttonHeight;

	void Awake()
	{
		ownTransform = transform;
		mainCamera = Camera.main;

		button = ownTransform.GetComponent<Button>();
		buttonRectTransform = ownTransform.GetComponent<RectTransform>();
		buttonWidth = buttonRectTransform.rect.width * buttonRectTransform.localScale.x;
		buttonHeight = buttonRectTransform.rect.height * buttonRectTransform.localScale.y;
	}

	void Start()
	{
		wavesManager = WavesManager.Instance;
		button.onClick.AddListener(wavesManager.StartWave);
		PrepareCicrleBar();
	}

	void Update()
	{
		MoveButton();
		SetCircleBarValue();
	}

	/// <summary>
	/// Подготовить прогресс бары
	/// </summary>
	public void PrepareCicrleBar()
	{
		circleBarMaxValue = Wave.startWaveTimer;
		currentCircleBarValue = circleBarMaxValue;
		circleBar.SetSettings(circleBarMaxValue, 0);
	}

	void MoveButton()
	{
		Vector3 targetPositionInScreen = mainCamera.WorldToScreenPoint(target.position);
		float xPos = targetPositionInScreen.x;
		float yPos = targetPositionInScreen.y;

		float cameraWidth = mainCamera.pixelWidth - 50;
		float cameraHeight = mainCamera.pixelHeight - 50;

		Vector2 screenCenter = new Vector2(cameraWidth / 2, cameraHeight / 2);

		Vector2 buttonCenter = new Vector2(xPos, yPos);
		Vector2 buttonBottomLeft = new Vector2(buttonCenter.x - buttonWidth / 2, buttonCenter.y - buttonHeight / 2);
		Vector2 buttonBottomRight = new Vector2(buttonCenter.x + buttonWidth / 2, buttonCenter.y - buttonHeight / 2);
		Vector2 buttonTopLeft = new Vector2(buttonCenter.x - buttonWidth / 2, buttonCenter.y + buttonHeight / 2);
		Vector2 buttonTopRight = new Vector2(buttonCenter.x + buttonWidth / 2, buttonCenter.y + buttonHeight / 2);

		ownTransform.position = new Vector3(xPos, yPos);

		// если находится в видимости камеры, то прикрепляем к объекту
		if (buttonBottomLeft.x >= 50 && buttonBottomLeft.y >= 50 &&
			buttonTopRight.x <= cameraWidth && buttonTopRight.y <= cameraHeight)
			return;

		float newXPos = 0;
		float newYPos = 0;

		float maxX = (cameraWidth * (targetPositionInScreen.y - screenCenter.y)) / cameraHeight + screenCenter.x;
		float minX = (-cameraWidth * (targetPositionInScreen.y - screenCenter.y)) / cameraHeight + screenCenter.x;

		float maxY = (targetPositionInScreen.x - screenCenter.x) * cameraHeight / cameraWidth + screenCenter.y;
		float minY = (targetPositionInScreen.x - screenCenter.x) * -cameraHeight / cameraWidth + screenCenter.y;

		// если первая четверть
		if (screenCenter.y < targetPositionInScreen.y && targetPositionInScreen.x > minX && targetPositionInScreen.x < maxX)
		{
			newXPos = (targetPositionInScreen.x - screenCenter.x) / (targetPositionInScreen.y - screenCenter.y) * ((cameraHeight - buttonHeight) / 2) + screenCenter.x;
			newYPos = (cameraHeight - buttonHeight) / 2 + screenCenter.y;
		}
		// если вторая четверть
		else if (screenCenter.x < targetPositionInScreen.x && targetPositionInScreen.y > minY && targetPositionInScreen.y < maxY)
		{
			newXPos = (cameraWidth - buttonWidth) / 2 + screenCenter.x;
			newYPos = ((targetPositionInScreen.y - screenCenter.y) / (targetPositionInScreen.x - screenCenter.x)) * ((cameraWidth - buttonWidth) / 2) + screenCenter.y;
		}
		// если третья четверть
		else if (screenCenter.y > targetPositionInScreen.y && targetPositionInScreen.x > maxX && targetPositionInScreen.x <= minX)
		{
			newXPos = screenCenter.x - (targetPositionInScreen.x - screenCenter.x) / (targetPositionInScreen.y - screenCenter.y) * ((cameraHeight - buttonHeight) / 2);
			newYPos = screenCenter.y - (cameraHeight - buttonHeight) / 2;
		}
		// если четвертая четверть
		else if (screenCenter.x > targetPositionInScreen.x && targetPositionInScreen.y > maxY && targetPositionInScreen.y < minX)
		{
			newXPos = screenCenter.x - (cameraWidth - buttonWidth) / 2;
			newYPos = screenCenter.y - (targetPositionInScreen.y - screenCenter.y) / (targetPositionInScreen.x - screenCenter.x) * ((cameraWidth - buttonWidth) / 2);
		}

		ownTransform.position = new Vector3(newXPos, newYPos);
	}

	void SetCircleBarValue()
	{
		//float currentWaveValue = Wave.startWaveTimer;
		//float neededValue = circleBarMaxValue - currentWaveValue;
		/*float neededValue = circleBarMaxValue - currentCircleBarValue;
		if (neededValue >= 0)
		{
			circleBar.SetSettings(circleBarMaxValue, neededValue);
			currentCircleBarValue -= Time.deltaTime;
		}*/
		circleBar.SetSettings(Wave.startWaveTimer, wavesManager.nextWaveStartTimer);
		currentCircleBarValue -= Time.deltaTime;
	}
}
