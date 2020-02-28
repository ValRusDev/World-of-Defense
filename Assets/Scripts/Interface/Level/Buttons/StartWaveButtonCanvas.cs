using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartWaveButtonCanvas : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;

	/// <summary>
	/// Префаб кнопки начала волны
	/// </summary>
	[Header("Префаб кнопки начала волны"), Tooltip("Префаб кнопки начала волны")]
	public Transform startWaveButtonPrefab;

	WavesManager wavesManager;

	List<Wave> Waves =>
		wavesManager.waves;

	public Wave Wave =>
		wavesManager.CurrentWave;

	List<StartWaveButton> startWaveButtons;

	void Awake()
	{
		ownTransform = transform;
	}

	void Start()
	{
		wavesManager = WavesManager.Instance;

		startWaveButtons = new List<StartWaveButton>();

		for (int i = 0; i < ownTransform.childCount; i++)
		{
			Transform child = ownTransform.GetChild(i);
			StartWaveButton startWaveButton = child.GetComponent<StartWaveButton>();
			if (startWaveButton != null)
				startWaveButtons.Add(startWaveButton);
		}
	}

	/// <summary>
	/// Подготовить кнопки старта войны
	/// </summary>
	public void PrepareStartButtons()
	{
		foreach (var startButton in startWaveButtons)
			startButton.target = null;

		var startWaveButtonPlaces = Wave.startWaveButtonPlaces;
		for (int i = 0; i < startWaveButtonPlaces.Count; i++)
		{
			var startWaveButtonPlace = startWaveButtonPlaces[i];

			StartWaveButton startWaveButton;
			if (i > startWaveButtons.Count - 1)
			{
				Transform startWaveButtonTransform = Transform.Instantiate(startWaveButtonPrefab, ownTransform);
				startWaveButtonTransform.position = Camera.main.WorldToScreenPoint(startWaveButtonPlace.position);

				startWaveButton = startWaveButtonTransform.GetComponent<StartWaveButton>();
				startWaveButtons.Add(startWaveButton);
			}
			else
				startWaveButton = startWaveButtons[i];

			startWaveButton.target = startWaveButtonPlace;
			startWaveButton.PrepareCicrleBar();
		}

		foreach (var startButton in startWaveButtons)
		{
			var target = startButton.target;
			startButton.gameObject.SetActive(target != null);
		}

		Wave.buttonsShowed = true;
	}
}
