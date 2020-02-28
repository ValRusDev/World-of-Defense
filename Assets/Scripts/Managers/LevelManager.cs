using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingletonPersistent<LevelManager>
{
	/// <summary>
	/// Вид уровня
	/// </summary>
	[Header("Вид уровня"), Tooltip("Вид уровня")]
	public LevelKind levelKind = LevelKind.Standart;

	/// <summary>
	/// Начальное количество жизней
	/// </summary>
	[Header("Жизни"), Tooltip("Начальное количество жизней")]
	public int startLivesCount;

	/// <summary>
	/// Текущее количество жизней
	/// </summary>
	[Tooltip("Текущее количество жизней")]
	public string currentLivesCount;

	/// <summary>
	/// Начальное золото
	/// </summary>
	[Header("Золото"), Tooltip("Начальное золото")]
	public int startGold;

	/// <summary>
	/// Текущее золото
	/// </summary>
	[Tooltip("Текущее золото")]
	public string currentGold;

	/// <summary>
	/// Номер текущей волны
	/// </summary>
	[Header("Волны"), Tooltip("Менеджер волн")]
	public WavesManager wavesManager;

	/// <summary>
	/// Выбранный объект
	/// </summary>
	[Header("Прочее"), SerializeField, Tooltip("Выбранный объект")]
	SelectingObject selectedObject;

	float prevGameSpeed;

	public delegate void GoldChangeHandler();
	public event GoldChangeHandler OnGoldChange;

	public delegate void LivesCountChangeHandler();
	public event LivesCountChangeHandler OnLivesCountChange;

	public SelectingObject SelectedObject
	{
		get
		{
			return selectedObject;
		}
		set
		{
			if (value != null)
			{
				selectedObject = value;
				selectedObject.ChangeSelect();
			}
			else
			{
				selectedObject.ChangeSelect();
				selectedObject = value;
			}
		}
	}

	public override void Awake()
	{
		base.Awake();

		InitializeStartStats();
	}

	void Start()
	{
		wavesManager = WavesManager.Instance;
	}

	/// <summary>
	/// Инициализация стартовых данных
	/// </summary>
	void InitializeStartStats()
	{
		currentGold = B64X.Encode(startGold.ToString());
		currentLivesCount = B64X.Encode(startLivesCount.ToString());
	}

	/// <summary>
	/// Добавить золото
	/// </summary>
	/// <param name="amount">Количество</param>
	public void AddGold(int amount)
	{
		// Расшифровываем
		string decodingGold = B64X.Decode(currentGold);
		int existsGold = int.Parse(decodingGold);

		// Прибавляем
		existsGold += amount;

		// Кодируем обратно
		string encodingGold = B64X.Encode(existsGold.ToString());
		currentGold = encodingGold;

		OnGoldChange.Invoke();
	}

	/// <summary>
	/// Задать скорость игры
	/// </summary>
	/// <param name="speed">Скорость</param>
	public void SetGameSpeed(float speed)
	{
		Time.timeScale = speed;
	}

	/// <summary>
	/// Поставить игру на паузу
	/// </summary>
	public void GamePause(GameObject pauseCanvas)
	{
		pauseCanvas.SetActive(true);
		prevGameSpeed = Time.timeScale;
		Time.timeScale = 0;
	}

	/// <summary>
	/// Убрать игру с паузы
	/// </summary>
	public void GameUnpause(GameObject pauseCanvas)
	{
		pauseCanvas.SetActive(false);
		Time.timeScale = prevGameSpeed;
	}

	/// <summary>
	/// Перезагрузить уровень
	/// </summary>
	public void ReloadLevel()
	{
		Time.timeScale = 1;
		var activeScene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(activeScene.name);
	}

	/// <summary>
	/// Потерять жизнь
	/// </summary>
	/// <param name="amount">Количество</param>
	public void LoseLife(int amount = 1)
	{
		string currentLiveDecodedStr = B64X.Decode(currentLivesCount);
		int currentLiveDecoded = int.Parse(currentLiveDecodedStr);
		currentLiveDecoded -= amount;

		currentLivesCount = B64X.Encode(currentLiveDecoded.ToString());

		OnLivesCountChange.Invoke();

		if (currentLiveDecoded == 0)
			EndGame();
	}

	public void EndGame()
	{
		print("Проиграл!");
	}

	public enum LevelKind
	{
		Standart,
		Endless
	}
}
