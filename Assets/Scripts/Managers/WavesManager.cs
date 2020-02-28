using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesManager : MonoBehaviourSingletonPersistent<WavesManager>
{
	[HideInInspector]
	public Transform ownTransform;

	/// <summary>
	/// Номер текущей волны
	/// </summary>
	[Header("Волны"), SerializeField, Tooltip("Номер текущей волны")]
	int currentWaveNumber;

	/// <summary>
	/// Номер текущей волны
	/// </summary>
	public int CurrentWaveNumber
	{
		get
		{
			return currentWaveNumber;
		}
		set
		{
			currentWaveNumber = value;
			OnWaveNumberChange.Invoke(WavesState);
		}
	}

	/// <summary>
	/// Текущая волна
	/// </summary>
	public Wave CurrentWave
	{
		get
		{
			if (waves == null || waves != null && waves.Count == 0)
				return null;

			if (CurrentWaveNumber > waves.Count - 1)
				return null;

			return waves[CurrentWaveNumber];
		}
	}

	/// <summary>
	/// Следующая волна
	/// </summary>
	public Wave NextWave
	{
		get
		{
			if (waves == null || waves != null && waves.Count == 0)
				return null;

			int nextWaveNumber = CurrentWaveNumber + 1;
			if (nextWaveNumber > waves.Count - 1)
				return null;

			return waves[nextWaveNumber];
		}
	}

	/// <summary>
	/// Количество волн
	/// </summary>
	[Tooltip("Волны")]
	public List<Wave> waves;

	public int WavesCount =>
		waves.Count;

	LevelManager levelManager;
	LevelManager.LevelKind LevelKind
	{
		get
		{
			if (levelManager == null)
				levelManager = LevelManager.Instance;

			return levelManager.levelKind;
		}
	}

	/// <summary>
	/// Текствое представление номера текущей волны и количества всех волн
	/// </summary>
	public string WavesState
	{
		get
		{
			if (LevelKind == LevelManager.LevelKind.Standart)
				return $"{currentWaveNumber + 1} / {WavesCount}";
			else
				return $"{currentWaveNumber + 1}";
		}
	}

	/// <summary>
	/// Canvas кнопки начала волны
	/// </summary>
	[Header("Canvas кнопки начала волны"), Tooltip("Canvas кнопки начала волны")]
	public StartWaveButtonCanvas startWaveButtonCanvas;

	/// <summary>
	/// Таймер до показа кнопки старта следующей волны
	/// </summary>
	[Header("Таймер до показа кнопки старта следующей волны"), Tooltip("Таймер до показа кнопки старта следующей волны")]
	public float nextWaveButtonsShowTimer;

	/// <summary>
	/// Таймер после показа кнопки начала волны и до старта волны
	/// </summary>
	[Header("Таймер после показа кнопки начала волны и до старта волны"), Tooltip("Таймер после показа кнопки начала волны и до старта волны")]
	public float nextWaveStartTimer;

	public delegate void WaveNumberChangeHandrer(string value);
	public event WaveNumberChangeHandrer OnWaveNumberChange;

	public override void Awake()
	{
		base.Awake();

		ownTransform = transform;
		waves = new List<Wave>();

		for (int i = 0; i < ownTransform.childCount; i++)
		{
			Transform child = ownTransform.GetChild(i);
			Wave wave = child.GetComponent<Wave>();
			if (wave != null)
			{
				wave.number = i;
				waves.Add(wave);
			}
		}
	}

	void Start()
	{
		if (levelManager == null)
			levelManager = LevelManager.Instance;

		if (OnWaveNumberChange != null)
			OnWaveNumberChange.Invoke(WavesState);
	}

	void Update()
	{
		NextWaveButtonsShowTick();
		NextWaveStartTimer();
	}

	/// <summary>
	/// Таймер до показа кнопки старта следующей волны
	/// </summary>
	void NextWaveButtonsShowTick()
	{
		if (CurrentWave == null)
			return;

		if (CurrentWave.buttonsShowed)
			return;

		if (nextWaveButtonsShowTimer >= CurrentWave.waveTimer)
		{
			PrepareNextWave();
			nextWaveButtonsShowTimer = 0;
		}
		else
			nextWaveButtonsShowTimer += Time.deltaTime;
	}

	/// <summary>
	/// Таймер после показа кнопки начала волны и до старта волны
	/// </summary>
	void NextWaveStartTimer()
	{
		if (CurrentWave == null)
			return;

		if (CurrentWave.startTimerIsDone)
			return;

		if (!CurrentWave.buttonsShowed)
			return;

		if (nextWaveStartTimer >= CurrentWave.startWaveTimer)
		{
			CurrentWave.startTimerIsDone = true;
			if (levelManager.levelKind == LevelManager.LevelKind.Endless && CurrentWaveNumber != 0)
				StartWave();
		}
		else
			nextWaveStartTimer += Time.deltaTime;
	}

	/// <summary>
	/// Запуск волны
	/// </summary>
	public void StartWave()
	{
		CurrentWave.gameObject.SetActive(true);
		if (CurrentWaveNumber < WavesCount - 1)
			CurrentWaveNumber++;
		startWaveButtonCanvas.gameObject.SetActive(false);
	}

	/// <summary>
	/// Подготовить следующую волну
	/// </summary>
	void PrepareNextWave()
	{
		startWaveButtonCanvas.gameObject.SetActive(true);
		startWaveButtonCanvas.PrepareStartButtons();
	}
}
