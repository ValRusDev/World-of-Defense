using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;

	/// <summary>
	/// Префаб врага
	/// </summary>
	[Header("Основное"), Tooltip("Префаб врага")]
	public Transform enemyPrefab;

	[Tooltip("Количество, которое нужно создать")]
	public int quantity;

	/// <summary>
	/// Путь, по которому пойдет враг
	/// </summary>
	[Tooltip("Путь, по которому пойдет враг")]
	public Way way;

	/// <summary>
	/// Первый waypoint
	/// </summary>
	Waypoint firstWaypoint;

	/// <summary>
	/// Текущее количество
	/// </summary>
	int currentQuantity;

	/// <summary>
	/// Интервал создания
	/// </summary>
	[Header("Интервал создания"), Tooltip("Интервал создания")]
	public float interval;

	/// <summary>
	///	Таймер интевала
	/// </summary>
	[SerializeField]
	float intervalTimer;

	/// <summary>
	/// Задержка перед стартом
	/// </summary>
	[Header("Интервал задержки"), Tooltip("Задержка перед стартом")]
	public float delay;

	/// <summary>
	///	Таймер задержки
	/// </summary>
	[SerializeField]
	float delayTimer;

	/// <summary>
	/// Созданные враги
	/// </summary>
	[Header("Враги"), Tooltip("Созданные враги")]
	public List<Enemy> createdEnemies;

	/// <summary>
	/// Может создавать или нет
	/// </summary>
	bool canCreate;

	/// <summary>
	/// Запущен или нет
	/// </summary>
	bool isStarted;

	void Awake()
	{
		ownTransform = transform;
		createdEnemies = new List<Enemy>();
	}

	void Start()
	{
		firstWaypoint = way.waypoints[0];
	}

	void Update()
	{
		DelayTimerTick();
		IntervalTimerTick();
	}

	void DelayTimerTick()
	{
		if (canCreate)
			return;

		if (delayTimer >= delay)
			canCreate = true;
		else
			delayTimer += Time.deltaTime;
	}

	void IntervalTimerTick()
	{
		if (!canCreate)
			return;

		if (currentQuantity >= quantity)
			return;

		if (!isStarted)
		{
			CreateEnemy();
			isStarted = true;
			return;
		}

		if (intervalTimer >= interval)
		{
			CreateEnemy();
			intervalTimer = 0;
		}
		else
			intervalTimer += Time.deltaTime;
	}

	void CreateEnemy()
	{
		Transform enemyTransform = Instantiate(enemyPrefab, ownTransform.position, Quaternion.identity);
		enemyTransform.parent = ownTransform.parent;

		Enemy enemy = enemyTransform.GetComponent<Enemy>();
		enemy.way = way;
		createdEnemies.Add(enemy);

		currentQuantity++;
		CheckQuantity();
	}

	void CheckQuantity()
	{
		if (currentQuantity >= quantity)
			gameObject.SetActive(false);
	}
}
