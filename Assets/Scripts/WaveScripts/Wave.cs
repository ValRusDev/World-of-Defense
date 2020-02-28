using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
	/// <summary>
	/// Время до показа кнопок старта волны
	/// </summary>
	[Tooltip("Время до показа кнопки старта волны")]
    public float waveTimer;

	/// <summary>
	/// Время до старта волны после показа кнопки
	/// </summary>
	[Tooltip("Время до старта волны после показа кнопки")]
	public float startWaveTimer;

	/// <summary>
	/// Номер волны
	/// </summary>
	[Tooltip("Номер волны")]
	public int number;

	/// <summary>
	/// Автоматический старт по истечении времени до старта волны
	/// </summary>
	bool autoStart =>
		levelManager.levelKind == LevelManager.LevelKind.Endless;

	/// <summary>
	/// Места для кнопок начала волны"
	/// </summary>
	[Tooltip("Места для кнопок начала волны")]
	public List<Transform> startWaveButtonPlaces;

	/// <summary>
	/// Кнопки показаны
	/// </summary>
	[Tooltip("Кнопки показаны")]
	public bool buttonsShowed;

	[Tooltip("Таймер после показа кнопкок и до старта волны завершен")]
	public bool startTimerIsDone;

	LevelManager levelManager;

    bool timerStarted;
    bool waveStarted;

	void Start()
	{
		levelManager = LevelManager.Instance;
	}

	/*void Update()
    {
		if (timerStarted && waveTimer > 0)
            waveTimer -= Time.deltaTime;

        if (!buttonsShowed && waveTimer <= 0)
            ShowStartButtons();

        if (timerStarted && buttonsShowed && startWaveTimer > 0)
            startWaveTimer -= Time.deltaTime;

        if (autoStart && timerStarted && startWaveTimer <= 0)
            StartWave();
	}*/

    public void StartWave()
    {
        timerStarted = false;

		levelManager.AddGold((int)startWaveTimer);
	}

    public void StartTimer()
    {
        timerStarted = true;
    }

    void ShowStartButtons()
    {
        
        buttonsShowed = true;
    }
}
