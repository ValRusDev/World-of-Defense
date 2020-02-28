using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarTower : Tower
{
	[Header("Свойства башни воинов")]
	/// <summary>
	/// Скорость передвижения воинов
	/// </summary>
	[Tooltip("Скорость передвижения воинов")]
	public float moveSpeed = 20;

	/// <summary>
	/// Здоровье воинов
	/// </summary>
	[Tooltip("Здоровье воинов")]
	public float heath = 30;

	/// <summary>
	/// Префаб воина
	/// </summary>
	[Tooltip("Префаб воина")]
	public Transform warUnitPrefab;

	/// <summary>
	/// Точка расположения воинов
	/// </summary>
	[Tooltip("Точка расположения воинов")]
	public PlacePoint placePoint;

	/// <summary>
	/// Область, куда могут перемещаться воины
	/// </summary>
	[Tooltip("Область, куда могут перемещаться воины")]
	public MoveArea moveArea;

	[Header("Ворота")]
	/// <summary>
	/// GO ворот
	/// </summary>
	[Tooltip("GO ворот")]
	public GameObject gate;

	/// <summary>
	/// Ворота открыты
	/// </summary>
	[Tooltip("Ворота открыты")]
	public bool gateIsOpened;

	/// <summary>
	/// Время открытых ворот
	/// </summary>
	[Tooltip("Время открытых ворот")]
	public float gateOpenTime = 2f;

	/// <summary>
	/// Время открытых ворот
	/// </summary>
	[Tooltip("Время открытых ворот")]
	public float gateOpenTimeTimer;

	[Header("Воины")]
	/// <summary>
	/// Родитель для копий войнов
	/// </summary>
	[Tooltip("Родитель для копий войнов")]
	public GameObject parentGameObject;

	/// <summary>
	/// Количество воинов
	/// </summary>
	[Tooltip("Количество воинов")]
	public byte warriowCount = 3;

	/// <summary>
	/// Список воинов
	/// </summary>
	[Tooltip("Список воинов")]
	public List<WarUnit> warriors;

	[Header("Восрешение")]
	/// <summary>
	/// Время воскрешения воина
	/// </summary>
	[Tooltip("Время воскрешения воина")]
	public float resurrectionTime = 50;

	/// <summary>
	/// Счетчик таймера времени воскрешения воина
	/// </summary>
	[Tooltip("Счетчик таймера времени воскрешения воина")]
	public float resurrectionTimeCount = 0;

	[Header("Прочее")]
	/// <summary>
	/// Игра начата или нет
	/// </summary>
	[Tooltip("Игра начата или нет")]
	public bool gameStarted;

	public delegate void WarriorCreated();
	public event WarriorCreated OnWarriorCreated;

	void Awake()
	{
		ownTransform = transform;
		FindNearestWaypoint();
		ammoSpawnPoint = transform;
	}

	void Start()
	{
		warriors = new List<WarUnit>();
	}

	void Update()
	{
		if (gameStarted)
		{
			if (warriors.Count < warriowCount)
			{
				if (resurrectionTimeCount >= resurrectionTime)
					CreateWarrior();
				else
					resurrectionTimeCount += Time.deltaTime;
			}
			else if (resurrectionTimeCount != 0)
				resurrectionTimeCount = 0;
		}
		else if (warriors.Count < warriowCount)
			CreateWarrior();

	}

	void LateUpdate()
	{
		CheckGate();
	}

	void CheckGate()
	{
		if (!gateIsOpened)
			return;

		if (gateOpenTimeTimer >= gateOpenTime)
			CloseGate();
		else
			gateOpenTimeTimer += Time.deltaTime;
	}

	/// <summary>
	/// Поиск ближайшей точки
	/// </summary>
	void FindNearestWaypoint()
	{
		Transform nearestWaypoint = null;
		var allWaypoints = FindObjectsOfType(typeof(Waypoint)).OfType<Waypoint>().Select(w => w.transform).Where(t => t.parent.name.Contains("_2")).ToArray();
		if (allWaypoints.Any())
		{
			float minDistance = 5000;
			foreach (var waypoint in allWaypoints)
			{
				float distance = Vector2.Distance(ownTransform.position, waypoint.position);
				if (distance < minDistance)
				{
					minDistance = distance;
					nearestWaypoint = waypoint;
				}
			}
		}
		else
			nearestWaypoint = placePoint.transform;

		placePoint.transform.position = nearestWaypoint.position;
	}

	/// <summary>
	/// Создать воина
	/// </summary>
	void CreateWarrior()
	{
		resurrectionTimeCount = 0;
		Transform warriorInst = Instantiate(warUnitPrefab, ammoSpawnPoint.transform.position, Quaternion.identity);
		warriorInst.parent = parentGameObject.transform;

		WarUnit warUnit = warriorInst.GetComponent<WarUnit>();
		warUnit.tower = this;
		warriors.Add(warUnit);

		OpenGate();
	}

	/// <summary>
	/// Открыть ворота
	/// </summary>
	void OpenGate()
	{
		if (!gateIsOpened)
		{
			gate.SetActive(false);
			gateIsOpened = true;
		}
		gateOpenTimeTimer = 0;
	}


	/// <summary>
	/// Закрыть ворота
	/// </summary>
	void CloseGate()
	{
		gate.SetActive(true);
		gateIsOpened = false;
	}

	public void SetPlacePoint(Vector3 position)
	{
		placePoint.transform.position = position;
		foreach (var warUnit in warriors)
		{
			if (warUnit.currentVortex != null)
				warUnit.stayPosition = warUnit.currentVortex.ownTransform.position;
			warUnit.NeedToGo = true;
		}
	}
}
