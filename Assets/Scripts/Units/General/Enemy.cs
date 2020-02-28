using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
	[Header("Путь")]
	/// <summary>
	/// Путь
	/// </summary>
	[Tooltip("Путь")]
	public Way way;

	/// <summary>
	/// Точки пути
	/// </summary>
	[Tooltip("Точки пути")]
	List<Waypoint> waypoints = new List<Waypoint>();

	/// <summary>
	/// Текущий индекс пути
	/// </summary>
	[Tooltip("Текущий индекс пути")]
	int currentRoutePoint = 0;

	/// <summary>
	/// Текущая точка пути
	/// </summary>
	[Tooltip("Текущая точка пути")]
	public Waypoint currentWaypoint;

	/// <summary>
	/// Следующая точка пути
	/// </summary>
	[Tooltip("Следующая точка пути")]
	public Waypoint nextWaypoint;

	[Header("Свойства врага")]
	/// <summary>
	/// Награда за убийство
	/// </summary>
	[Tooltip("Награда за убийство")]
	public short reward;

	LevelManager levelManager;

	void Awake()
	{
        SetStartStats();
	}

	void Start()
    {
		levelManager = LevelManager.Instance;

        SetWaypoints();
        ShowHideMeshs(false);
    }

    void Update()
    {
		if (isDied)
			return;

		if (target != null)
        {
			CurrentMoveSpeed = 0;
            if (IsNearEnemy())
                MeleeAttack();
        }
        else
            Move();

    }

    void LateUpdate()
    {
		CheckDie();

		if (isDied)
			return;

		//health.SetHealthBar();
		CheckHealth();
	}

	/*public override bool IsNearEnemy()
	{
		float minDistane = 2.0f;
		if (ownRenderer != null && target.ownRenderer != null)
			minDistane = ownRenderer.bounds.size.x / 2 + target.ownRenderer.bounds.size.x / 2;

		float distance = Vector3.Distance(ownTransform.position, target.ownTransform.position);
		return distance < minDistane;
	}*/

	/// <summary>
	/// Идти к точке
	/// </summary>
	void Move()
    {
		if (currentMoveSpeed == 0)
			CurrentMoveSpeed = moveSpeed;

        CheckReverse();
		if (CheckWaypointPosition())
		{
			LookAt(nextWaypoint.ownTransfrom.position);
			ownTransform.position = Vector3.MoveTowards(ownTransform.position, nextWaypoint.ownTransfrom.position, currentMoveSpeed * Time.deltaTime);
		}
    }

	/// <summary>
	/// Определить точки пути
	/// </summary>
	/// <returns></returns>
    bool CheckWaypointPosition()
    {
		Vector3 direction = Vector3.zero;
		direction = nextWaypoint.ownTransfrom.position - ownTransform.position;
		float magnitude = direction.magnitude;
		if (magnitude < 2.0f)
        {
            var waypointComponent = nextWaypoint.GetComponent<Waypoint>();
            if (waypointComponent.IsStart)
                isActive = true;

            if (waypointComponent.IsFinish)
            {
				isActive = false;
				TakeAwayLife();
				Die();
                return false;
            }

            if (currentRoutePoint >= waypoints.Count - 1)
                return false;

            currentWaypoint = waypoints[currentRoutePoint];
            currentRoutePoint++;

            nextWaypoint = waypoints[currentRoutePoint];
        }
		return true;
    }

	/// <summary>
	/// Проверить идет ли обратно
	/// </summary>
    void CheckReverse()
    {
        if (reverseMove)
        {
            if (reverseCurrentTime <= reverseTime)
                reverseCurrentTime += Time.deltaTime;
            else
            {
                reverseCurrentTime = 0;
                reverseMove = false;
                SwapWay();
            }
        }
    }

	/// <summary>
	/// Поменять пути при реверсе
	/// </summary>
    void SwapWay()
    {
        // инверсируем массив массив
        waypoints.Reverse();

        // меняем индекс текущей точки пути
        int allWaypointsCount = waypoints.Count;
        currentRoutePoint = allWaypointsCount - 1 - currentRoutePoint;
        currentWaypoint = waypoints[currentRoutePoint];
        nextWaypoint = waypoints[currentRoutePoint + 1];
        /*Transform tempTransform = currentWaypoint;
        currentWaypoint = nextWaypoint;
        nextWaypoint = tempTransform;*/
        //currentWaypoint = waypoints[currentRoutePoint];
        //nextWaypoint = waypoints[currentRoutePoint + 1];

    }


    /// <summary>
	/// Если дошёл до конца, отнимаем жизнь
	/// </summary>
    void TakeAwayLife()
    {
		levelManager.LoseLife();
    }

	/// <summary>
	/// Задать точки пути
	/// </summary>
    void SetWaypoints()
    {
        //way = transform.parent.GetComponent<UnitsPack>().way;
		waypoints = way.waypoints;

		if (waypoints.Count != 0)
            nextWaypoint = waypoints[0];
    }


    /// <summary>
    /// Испугаться
    /// </summary>
    public void GetScared(float seconds)
    {
        if (!reverseMove)
        {
            reverseTime = seconds;
            reverseMove = true;
            SwapWay();
        }
        else
            reverseCurrentTime = 0;
    }

	/// <summary>
	/// Начало смерти
	/// </summary>
	public override void StartDie()
	{
		levelManager.AddGold(reward);

		base.StartDie();
	}

	/// <summary>
	/// Окончательная смерть
	/// </summary>
	protected override void Die()
	{
		base.Die();

		Destroy(gameObject);
	}
}