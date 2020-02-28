using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
	[Header("Настройки камеры")]

	/// <summary>
	/// Текущая камера
	/// </summary>
	[Tooltip("Текущая камера")]
	Camera currentCamera;

	/// <summary>
	/// Скорость камеры
	/// </summary>
	[Tooltip("Скорость камеры")]
	public float cameraSpeed;

	/// <summary>
	/// Количество золота
	/// </summary>
	[Tooltip("Количество золота")]
	public static string gold;
	/// <summary>
	/// Включена пауза
	/// </summary>
	[Tooltip("Включена пауза")]
	public static bool inPause;

	[HideInInspector]
	public Transform ownTransform;

	[Header("Задний фон")]
	/// <summary>
	/// Задний фон
	/// </summary>
	[Tooltip("Задний фон")]
	public Transform background;

	/// <summary>
	/// Спрайт заднего фона
	/// </summary>
	[Tooltip("Спрайт заднего фона")]
	SpriteRenderer backgroundSR;

	[Header("Пауза")]
	/// <summary>
	/// Канвас паузы
	/// </summary>
	[Tooltip("Канвас паузы")]
	public Transform pauseCanvas;

	[Header("Золото")]
	/// <summary>
	/// Начальное золото
	/// </summary>
	[Tooltip("Начальное золото")]
	public short startedGold;

	/// <summary>
	/// Объект, отображающий золото
	/// </summary>
	[Tooltip("Объект, отображающий золото")]
	public GameObject goldDisplay;

	[Header("Жизни")]
	/// <summary>
	/// Количество жизней
	/// </summary>
	[Tooltip("Количество жизней")]
	public byte livesCount = 20;

	/// <summary>
	/// Объект, отображающий количество жизней
	/// </summary>
	[Tooltip("Объект, отображающий количество жизней")]
	public GameObject livesCountText;

	[Header("Волны")]
	//public float nextWaveTimer;
	/// <summary>
	/// Объект, отображающий волны
	/// </summary>
	[Tooltip("Объект, отображающий волны")]
	public GameObject wavesDisplay;

	/// <summary>
	/// Номер текущей волны
	/// </summary>
	[Tooltip("Номер текущей волны")]
	public ushort currentWave;

	/// <summary>
	/// Список волн
	/// </summary>
	[Tooltip("Список волн")]
	public Wave[] waveList;

	/// <summary>
	/// Количество волн
	/// </summary>
	[Tooltip("Количество волн")]
	ushort wavesCount;

	[Header("Герои")]
	/// <summary>
	/// Список героев
	/// </summary>
	[Tooltip("Список героев")]
	public List<Hero> heroes = new List<Hero>();

	[Header("Панели")]
	/// <summary>
	/// Панель героев
	/// </summary>
	[Tooltip("Панель героев")]
	public Transform heroesListPanel;

	/// <summary>
	/// Панель способностей
	/// </summary>
	[Tooltip("Панель способностей")]
	public Transform spellsPanel;

	[Header("Прочее")]
	/// <summary>
	/// Выбранный объект
	/// </summary>
	[Tooltip("Выбранный объект")]
	public Transform selectedObject;

	/// <summary>
	/// Игра начата или нет
	/// </summary>
	[Tooltip("Игра начата или нет")]
	public bool gameStarted;

	/// <summary>
	/// Нажата кнопка (КОСТЫЛЬ :с )
	/// </summary>
	[Tooltip("Нажата кнопка (КОСТЫЛЬ :с )")]
	public bool buttonPressed;

	Vector2?[] oldTouchPositions = { null, null };
	Vector2 oldTouchVector;
	float oldTouchDistance;

	/*Touch initTouch = new Touch();
    Touch zoomTouch1 = new Touch();
    Touch zoomTouch2 = new Touch();
    float moveSpeed = 0.05f;*/

	[Header("Настойки приближения")]
	/// <summary>
	/// Максимальное отдаление
	/// </summary>
	[Tooltip("Максимальное отдаление")]
	public float maxCameraSize;

	/// <summary>
	/// Максимальное приближение
	/// </summary>
	[Tooltip("Максимальное приближение")]
	public float minCameraSize;

	void Awake()
	{
		SetHeroesToButton();
		SetSpeelToButton();
		ownTransform = transform;
		gold = B64X.Encode(startedGold.ToString());
		backgroundSR = background.GetComponent<SpriteRenderer>();
		currentCamera = ownTransform.GetComponent<Camera>();
		maxCameraSize = currentCamera.orthographicSize;
		minCameraSize = maxCameraSize - 1f;
	}

	void Start()
	{
		WriteGoldCount();
		WriteWaves();
		PrepareNextWave();
	}

	void Update()
	{
		if (!buttonPressed)
			Touch();
		else
			buttonPressed = false;
		//GetInput();
	}

	void GetInput()
	{
		if (Input.GetKey(KeyCode.W))
		{
			ownTransform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))
		{
			ownTransform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D))
		{
			ownTransform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A))
		{
			ownTransform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetMouseButtonUp(0))
		{
			var mousePosition = Input.mousePosition;
			var touchPosWorld = Camera.main.ScreenToWorldPoint(mousePosition);
			Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
			DoActionWithObject(touchPosWorld2D);
		}
	}

	/// <summary>
	/// Совершить действия с объектом
	/// </summary>
	/// <param name="touchPosWorld2D">Точка в мировом пространстве</param>
	void DoActionWithObject(Vector2 touchPosWorld2D)
	{
		var touch = Input.GetTouch(0);
		Transform findedObject = FindObjectFromPosition(touchPosWorld2D);
		if (selectedObject != null)
		{
			if (touch.phase == TouchPhase.Ended)
			{
				// проверяем активность способносности
				var spellButtonComponent = selectedObject.GetComponent<SpellButton>();
				if (spellButtonComponent != null)
				{
					var hero = spellButtonComponent.hero;
					var heroComponent = hero.GetComponent<Hero>();
					heroComponent.positionForSpell = touchPosWorld2D;
					heroComponent.Cast();
				}
				else
				{
					if (findedObject != null)
					{
						if (findedObject != selectedObject)
						{
							selectedObject.GetComponent<SelectingObject>().ChangeSelect();
							SetFindedObject(findedObject);
						}
					}
					else
					{
						var heroComponent = selectedObject.GetComponent<Hero>();
						if (heroComponent != null)
						{
							heroComponent.GoToPosition(touchPosWorld2D);
							selectedObject.GetComponent<SelectingObject>().ChangeSelect();
						}
						else
						{
							selectedObject.GetComponent<SelectingObject>().ChangeSelect();
						}
					}
				}
			}
		}
		else
		{
			if (findedObject != null)
			{
				if (touch.phase == TouchPhase.Ended)
				{
					SetFindedObject(findedObject);
				}
			}
			else
			{
				if (touch.phase == TouchPhase.Moved)
				{
					MoveCamera();
				}
			}
		}
	}

	/// <summary>
	/// Задать выбранным найденный объект
	/// </summary>
	/// <param name="findedObject">Найденный объект</param>
	void SetFindedObject(Transform findedObject)
	{
		selectedObject = findedObject;
		selectedObject.GetComponent<SelectingObject>().ChangeSelect();
	}

	/// <summary>
	/// Найти объект по точке в пространствен
	/// </summary>
	/// <param name="touchPosWorld2D"></param>
	/// <returns></returns>
	Transform FindObjectFromPosition(Vector2 touchPosWorld2D)
	{
		Transform findedObject = null;
		var hitInformations = Physics2D.RaycastAll(touchPosWorld2D, Camera.main.transform.forward);
		foreach (var hitInformation in hitInformations)
		{
			if (hitInformation.transform != null)
			{
				Transform hitTransform = hitInformation.transform;
				var mainObjectComponent = hitTransform.GetComponent<SelectingObject>();
				if (mainObjectComponent != null)
				{
					var heroComponent = hitTransform.GetComponent<Hero>();
					var towerComponent = hitTransform.GetComponent<Tower>();
					var buildPlaceComponent = hitTransform.GetComponent<BuildPlace>();
					if (heroComponent != null)
					{
						findedObject = hitTransform;
						break;
					}
					else if (towerComponent != null)
					{
						findedObject = hitTransform;
						break;
					}
					else if (buildPlaceComponent != null)
					{
						findedObject = hitTransform;
						break;
					}
				}
			}
		}
		return findedObject;
	}

	/// <summary>
	/// Прикосновение
	/// </summary>
	void Touch()
	{
		if (Input.touchCount == 0)
		{
			oldTouchPositions[0] = null;
			oldTouchPositions[1] = null;
		}
		else if (Input.touchCount == 1)
		{
			if (oldTouchPositions[0] == null || oldTouchPositions[1] != null)
			{
				oldTouchPositions[0] = Input.GetTouch(0).position;
				oldTouchPositions[1] = null;
			}
			else
			{
				MoveOrSelectTouch();
			}
		}
		else
		{
			if (oldTouchPositions[1] == null)
			{
				oldTouchPositions[0] = Input.GetTouch(0).position;
				oldTouchPositions[1] = Input.GetTouch(1).position;
				oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
				oldTouchDistance = oldTouchVector.magnitude;
			}
			else
			{
				ZoomTouch();
			}
		}
	}

	/// <summary>
	/// Увеличение
	/// </summary>
	void ZoomTouch()
	{
		float width = currentCamera.pixelWidth;
		float height = currentCamera.pixelHeight;
		Vector2 screen = new Vector2(width, height);

		Vector2[] newTouchPositions = new Vector2[]
		{
					Input.GetTouch(0).position,
					Input.GetTouch(1).position
		};
		Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
		float newTouchDistance = newTouchVector.magnitude;
		float newSize = currentCamera.orthographicSize * oldTouchDistance / newTouchDistance;

		if (newSize > minCameraSize && newSize < maxCameraSize)
		{
			Vector2 tempVector = (Vector2)(oldTouchPositions[0] + oldTouchPositions[1] - screen);
			Vector3 directionVector = tempVector * currentCamera.orthographicSize / height;

			currentCamera.orthographicSize *= oldTouchDistance / newTouchDistance;

			float deltaX = 0;
			float deltaY = 0;

			Vector2 bottomLeft = currentCamera.ScreenToWorldPoint(new Vector2(0, 0));
			//Vector2 bottomRight = currentCamera.ScreenToWorldPoint(new Vector2(width, 0));
			Vector2 topLeft = currentCamera.ScreenToWorldPoint(new Vector2(0, height));
			Vector2 topRight = currentCamera.ScreenToWorldPoint(new Vector2(width, height));

			float backgroundSRWidth = backgroundSR.bounds.size.x;
			float backgroundSRHeight = backgroundSR.bounds.size.y;
			float backgroundMinX = background.position.x - backgroundSRWidth / 2;
			float backgroundMaxX = background.position.x + backgroundSRWidth / 2;
			float backgroundMinY = background.position.y - backgroundSRHeight / 2;
			float backgroundMaxY = background.position.y + backgroundSRHeight / 2;

			//float maxTopDistance = backgroundMaxY - topLeft.y;
			//float maxBottomDistance = bottomLeft.y - backgroundMinY;
			//float maxLeftDistance = topLeft.x - backgroundMinX;
			//float maxRightDistance = backgroundMaxX - topRight.x;

			// если ушло вверх
			if (topLeft.y > backgroundMaxY)
				deltaY = backgroundMaxY - topLeft.y;
			// если ушло вниз
			if (bottomLeft.y < backgroundMinY)
				deltaY = backgroundMinY - bottomLeft.y;
			// если ушло влево
			if (topLeft.x < backgroundMinX)
				deltaX = backgroundMinX - topLeft.x;
			// если ушло вправо
			if (topRight.x > backgroundMaxX)
				deltaX = backgroundMaxX - topRight.x;

			directionVector = new Vector3(deltaX, deltaY, 0);
			ownTransform.position += ownTransform.TransformDirection(directionVector);
		}

		oldTouchPositions[0] = newTouchPositions[0];
		oldTouchPositions[1] = newTouchPositions[1];
		oldTouchVector = newTouchVector;
		oldTouchDistance = newTouchDistance;
	}

	/// <summary>
	/// Перемешение или выбор
	/// </summary>
	void MoveOrSelectTouch()
	{
		var touch = Input.GetTouch(0);

		var touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
		Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
		DoActionWithObject(touchPosWorld2D);

	}

	/// <summary>
	/// Перемещение камеры
	/// </summary>
	void MoveCamera()
	{
		var touch = Input.GetTouch(0);
		Vector2 newTouchPosition = touch.position;

		float width = currentCamera.pixelWidth;
		float height = currentCamera.pixelHeight;


		Vector2 bottomLeft = currentCamera.ScreenToWorldPoint(new Vector2(0, 0));
		//Vector2 bottomRight = currentCamera.ScreenToWorldPoint(new Vector2(width, 0));
		Vector2 topLeft = currentCamera.ScreenToWorldPoint(new Vector2(0, height));
		Vector2 topRight = currentCamera.ScreenToWorldPoint(new Vector2(width, height));

		float backgroundSRWidth = backgroundSR.bounds.size.x;
		float backgroundSRHeight = backgroundSR.bounds.size.y;
		float backgroundMinX = background.position.x - backgroundSRWidth / 2;
		float backgroundMaxX = background.position.x + backgroundSRWidth / 2;
		float backgroundMinY = background.position.y - backgroundSRHeight / 2;
		float backgroundMaxY = background.position.y + backgroundSRHeight / 2;

		float maxTopDistance = backgroundMaxY - topLeft.y;
		float maxBottomDistance = bottomLeft.y - backgroundMinY;
		float maxLeftDistance = topLeft.x - backgroundMinX;
		float maxRightDistance = backgroundMaxX - topRight.x;

		float cameraSize = currentCamera.orthographicSize;

		Vector2 touchPositionDif = (Vector2)oldTouchPositions[0] - newTouchPosition;
		Vector3 directionVector = touchPositionDif * cameraSize / height * 2f;

		float directionX = directionVector.x;
		float directionY = directionVector.y;

		// если камера двигается вверх
		if (directionY > 0)
		{
			if (directionY > maxTopDistance)
				directionY = maxTopDistance;
		}
		// если камера двигается вниз
		if (directionY < 0)
		{
			float positiveDirectionY = directionY * (-1);
			if (positiveDirectionY > maxBottomDistance)
			{
				positiveDirectionY = maxBottomDistance;
				directionY = positiveDirectionY * (-1);
			}
		}
		// если камера двигается влево
		if (directionX < 0)
		{
			float positiveDirectionX = directionX * (-1);
			if (positiveDirectionX > maxLeftDistance)
			{
				positiveDirectionX = maxLeftDistance;
				directionX = positiveDirectionX * (-1);
			}
		}
		// если камера двигается вправо
		if (directionX > 0)
		{
			if (directionX > maxRightDistance)
				directionX = maxRightDistance;
		}

		directionVector = new Vector3(directionX, directionY, 0);
		ownTransform.position += ownTransform.TransformDirection(directionVector);
		oldTouchPositions[0] = newTouchPosition;
	}

	/// <summary>
	/// Забрать жизни
	/// </summary>
	/// <param name="count">Количество</param>
	public void LoseLife(int count = 1)
	{
		livesCount = (byte)(livesCount - 1);
		livesCountText.GetComponent<Text>().text = livesCount.ToString();

		if (livesCount == 0)
		{
			EndGame();
		}
	}

	void EndGame()
	{

	}

	/// <summary>
	/// Добавить золото
	/// </summary>
	/// <param name="goldToAdd">Количество золота</param>
	[Obsolete("Используйте GameManager")]
	public void AddGold(short goldToAdd)
	{
		string decodingGold = B64X.Decode(gold);
		short existsGold = short.Parse(decodingGold);
		existsGold += goldToAdd;
		gold = B64X.Encode(existsGold.ToString());
		WriteGoldCount();
	}

	/// <summary>
	/// Отобразить количество золота
	/// </summary>
	void WriteGoldCount()
	{
		goldDisplay.GetComponent<Text>().text = B64X.Decode(gold);
	}

	/// <summary>
	/// Пауза/возобновление игры
	/// </summary>
	public void PlayPauseGame()
	{
		if (Time.timeScale == 1)
		{
			Time.timeScale = 0;
			inPause = true;
		}
		else
		{
			Time.timeScale = 1;
			inPause = false;
		}
		pauseCanvas.gameObject.SetActive(inPause);
	}

	/// <summary>
	/// Ускорение игры
	/// </summary>
	public void BoostGame()
	{
		if (Time.timeScale == 1)
		{
			Time.timeScale = 2;
		}
		else if (Time.timeScale == 2)
		{
			Time.timeScale = 1;
		}
	}

	public void ExitLevelAndLoadMap()
	{
		//SceneManager.LoadScene("Map1");
	}

	/// <summary>
	/// Отобразить количество волн
	/// </summary>
	public void WriteWaves()
	{
		wavesCount = (ushort)waveList.Length;
		string currentWaverStr = currentWave.ToString();
		if (currentWave == 0)
			currentWaverStr = "1";
		string result = String.Format("{0} / {1}", currentWaverStr, wavesCount);
		wavesDisplay.GetComponent<Text>().text = result;

		PrepareNextWave();
	}

	/// <summary>
	/// Подготовить следующую волну
	/// </summary>
	void PrepareNextWave()
	{
		RefreshAll();

		if (currentWave >= wavesCount)
			return;

		Wave nextWave = waveList[currentWave];
		nextWave.gameObject.SetActive(true);
		nextWave.StartTimer();
	}

	/// <summary>
	/// Вылечить героев и юнитов
	/// </summary>
	void RefreshAll()
	{
		RefreshHeroes();
		RefreshUnits();
	}

	/// <summary>
	/// Вылечить героев и сбросить счетчики скиллов
	/// </summary>
	void RefreshHeroes()
	{
		HealHeroes();
		RefreshHeroesSkills();
	}

	/// <summary>
	/// Вылечить героев
	/// </summary>
	void HealHeroes()
	{
		foreach (var hero in heroes)
		{
			hero.health.FullHeal();
		}
	}

	/// <summary>
	/// Сбросить счетчики скиллов
	/// </summary>
	void RefreshHeroesSkills()
	{
		foreach (var hero in heroes)
		{
			hero.ResetSkillsCounters();
		}
	}

	/// <summary>
	/// Обновить юниты
	/// </summary>
	void RefreshUnits()
	{
		var warTowers = FindObjectsOfType<WarTower>();
		foreach (var warTower in warTowers)
		{
			var warriors = warTower.warriors;
			foreach (var warrior in warriors)
			{
				if (warrior != null)
				{
					WarUnit warUnit = warrior.GetComponent<WarUnit>();
					warUnit.health.FullHeal();
					warUnit.ResetSkillsCounters();
				}
			}
		}
	}

	/// <summary>
	/// Задать кнопки для героев
	/// </summary>
	void SetHeroesToButton()
	{
		List<Hero> heroesForButtons = new List<Hero>();
		heroesForButtons.AddRange(heroes.OrderBy(h => h.name));

		for (int i = 0; i < heroesListPanel.childCount; i++)
		{
			Transform heroButtonTransform = heroesListPanel.GetChild(i);
			Hero hero = heroesForButtons.FirstOrDefault();
			if (hero != null)
			{
				Button heroButton = heroButtonTransform.GetComponent<Button>();
				var heroButtonText = heroButton.GetComponentsInChildren<Text>().FirstOrDefault();
				heroButtonText.text = hero.name;

				var heroButtonComponent = heroButtonTransform.GetComponent<HeroButton>();
				heroButtonComponent.hero = hero;

				heroesForButtons.Remove(hero);
			}
			else
				Destroy(heroButtonTransform.gameObject);
		}
	}

	/// <summary>
	/// Задать кнопки для способностей героев
	/// </summary>
	void SetSpeelToButton()
	{
		List<Hero> heroesForSpellsButtons = new List<Hero>();
		heroesForSpellsButtons.AddRange(heroes.OrderBy(h => h.name));

		for (int i = 0; i < spellsPanel.childCount; i++)
		{
			Transform spellButtonTransform = spellsPanel.GetChild(i);
			Hero hero = heroesForSpellsButtons.FirstOrDefault();
			if (hero != null)
			{
				Button heroButton = spellButtonTransform.GetComponent<Button>();
				var heroButtonText = heroButton.GetComponentsInChildren<Text>().FirstOrDefault();
				heroButtonText.text = hero.name;

				var spellButtonComponent = spellButtonTransform.GetComponent<SpellButton>();
				spellButtonComponent.hero = hero;

				var heroComponent = hero.GetComponent<Hero>();
				heroComponent.spellButton = spellButtonTransform;

				heroesForSpellsButtons.Remove(hero);
			}
			else
				Destroy(spellButtonTransform.gameObject);
		}
	}

	#region Старые медоты
	/*
	void OldZoomTouch()
	{
		var touches = Input.touches.OrderBy(t => t.position.x);
		var touch1 = touches.First();
		var touch2 = touches.Last();

		if (touch1.phase == TouchPhase.Began || touch1.phase == TouchPhase.Stationary)
		{
			zoomTouch1 = touch1;
		}
		else if (touch1.phase == TouchPhase.Ended)
		{
			zoomTouch1 = new Touch();
		}
		if (touch2.phase == TouchPhase.Began || touch2.phase == TouchPhase.Stationary)
		{
			zoomTouch2 = touch2;
		}
		else if (touch2.phase == TouchPhase.Ended)
		{
			zoomTouch2 = new Touch();
		}

		if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
		{
			float distance1 = zoomTouch1.position.x - touch1.position.x;
			float distance2 = touch2.position.x - zoomTouch2.position.x;
			float totalDistance = distance1 + distance2;

			float currentSize = currentCamera.orthographicSize;
			float newSize = currentSize + totalDistance * moveSpeed * Time.deltaTime;

			// если приближаем
			if (totalDistance < 0)
			{
				if (newSize >= minCameraSize)
					currentCamera.orthographicSize = newSize;
			}
			// если отдаляем
			else if (totalDistance > 0)
			{
				if (newSize <= maxCameraSize)
				{
					currentCamera.orthographicSize = newSize;

					float deltaX = 0;
					float deltaY = 0;

					float width = currentCamera.pixelWidth;
					float height = currentCamera.pixelHeight;

					Vector2 bottomLeft = currentCamera.ScreenToWorldPoint(new Vector2(0, 0));
					Vector2 bottomRight = currentCamera.ScreenToWorldPoint(new Vector2(width, 0));
					Vector2 topLeft = currentCamera.ScreenToWorldPoint(new Vector2(0, height));
					Vector2 topRight = currentCamera.ScreenToWorldPoint(new Vector2(width, height));

					float backgroundSRWidth = backgroundSR.bounds.size.x;
					float backgroundSRHeight = backgroundSR.bounds.size.y;
					float backgroundMinX = background.position.x - backgroundSRWidth / 2;
					float backgroundMaxX = background.position.x + backgroundSRWidth / 2;
					float backgroundMinY = background.position.y - backgroundSRHeight / 2;
					float backgroundMaxY = background.position.y + backgroundSRHeight / 2;

					float maxTopDistance = backgroundMaxY - topLeft.y;
					float maxBottomDistance = bottomLeft.y - backgroundMinY;
					float maxLeftDistance = topLeft.x - backgroundMinX;
					float maxRightDistance = backgroundMaxX - topRight.x;

					// если ушло вверх
					if (topLeft.y > backgroundMaxY)
						deltaY = backgroundMaxY - topLeft.y;
					// если ушло вниз
					if (bottomLeft.y < backgroundMinY)
						deltaY = backgroundMinY - bottomLeft.y;
					// если ушло влево
					if (topLeft.x < backgroundMinX)
						deltaX = backgroundMinX - topLeft.x;
					// если ушло вправо
					if (topRight.x > backgroundMaxX)
						deltaX = backgroundMaxX - topRight.x;

					Vector3 direction = new Vector3(deltaX, deltaY, 0);
					ownTransform.Translate(direction);
				}
			}
		}
	}

	void OldMoveOrSelectTouch()
    {
        var touch = Input.touches.First();
        switch (touch.phase)
        {
            case TouchPhase.Began:
                {
                    initTouch = touch;
                    break;
                }
            case TouchPhase.Moved:
                {
                    cameraMove = true;

                    float width = currentCamera.pixelWidth;
                    float height = currentCamera.pixelHeight;

                    Vector2 bottomLeft = currentCamera.ScreenToWorldPoint(new Vector2(0, 0));
                    Vector2 bottomRight = currentCamera.ScreenToWorldPoint(new Vector2(width, 0));
                    Vector2 topLeft = currentCamera.ScreenToWorldPoint(new Vector2(0, height));
                    Vector2 topRight = currentCamera.ScreenToWorldPoint(new Vector2(width, height));

                    float backgroundSRWidth = backgroundSR.bounds.size.x;
                    float backgroundSRHeight = backgroundSR.bounds.size.y;
                    float backgroundMinX = background.position.x - backgroundSRWidth / 2;
                    float backgroundMaxX = background.position.x + backgroundSRWidth / 2;
                    float backgroundMinY = background.position.y - backgroundSRHeight / 2;
                    float backgroundMaxY = background.position.y + backgroundSRHeight / 2;

                    float maxTopDistance = backgroundMaxY - topLeft.y;
                    float maxBottomDistance = bottomLeft.y - backgroundMinY;
                    float maxLeftDistance = topLeft.x - backgroundMinX;
                    float maxRightDistance = backgroundMaxX - topRight.x;

                    float deltaX = initTouch.position.x - touch.position.x;
                    float deltaY = initTouch.position.y - touch.position.y;

                    // скорость движения камеры зависит от размера камеры
                    float newMoveSpeed = moveSpeed * currentCamera.orthographicSize / maxCameraSize;

                    float directionX = deltaX * newMoveSpeed * Time.deltaTime;
                    float directionY = deltaY * newMoveSpeed * Time.deltaTime;

                    // если камера двигается вверх
                    if (directionY > 0)
                    {
                        if (directionY > maxTopDistance)
                            directionY = maxTopDistance;
                    }
                    // если камера двигается вниз
                    if (directionY < 0)
                    {
                        float positiveDirectionY = directionY * (-1);
                        if (positiveDirectionY > maxBottomDistance)
                        {
                            positiveDirectionY = maxBottomDistance;
                            directionY = positiveDirectionY * (-1);
                        }
                    }
                    // если камера двигается влево
                    if (directionX < 0)
                    {
                        float positiveDirectionX = directionX * (-1);
                        if (positiveDirectionX > maxLeftDistance)
                        {
                            positiveDirectionX = maxLeftDistance;
                            directionX = positiveDirectionX * (-1);
                        }
                    }
                    // если камера двигается вправо
                    if (directionX > 0)
                    {
                        if (directionX > maxRightDistance)
                            directionX = maxRightDistance;
                    }
                    Vector3 direction = new Vector3(directionX, directionY, 0);
                    ownTransform.Translate(direction);
                    break;
                }
            case TouchPhase.Stationary:
                {
                    initTouch = touch;
                    break;
                }
            case TouchPhase.Ended:
                {
                    if (cameraMove)
                    {
                        initTouch = new Touch();
                        cameraMove = false;
                    }
                    else
                    {
                        var touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
                        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                        DoActionWithObject(touchPosWorld2D);
                    }
                    break;
                }
            case TouchPhase.Canceled:
                {
                    break;
                }
            default:
                break;
        }
    }*/
	#endregion
}

