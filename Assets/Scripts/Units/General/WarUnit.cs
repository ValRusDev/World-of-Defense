using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarUnit : Ally
{
	[Header("Свойства воина")]
	/// <summary>
	/// Башня, к которой принадлежит воин
	/// </summary>
	[Tooltip("Башня, к которой принадлежит воин")]
	public WarTower tower;

	/// <summary>
	/// Текущая вершина
	/// </summary>
	[Tooltip("Текущая вершина")]
	public VortexScript currentVortex;

	/// <summary>
	/// Точка возрождения
	/// </summary>
	[Tooltip("Точка возрождения")]
	Transform spawnPoint;

	/// <summary>
	/// Необходимость обновления статистик
	/// </summary>
	[Tooltip("Необходимость обновления статистик")]
	bool needUpdate = true;

	/// <summary>
	/// Ширина воина
	/// </summary>
	[Tooltip("Ширина воина")]
	float warriorWidth;

	/// <summary>
	/// Ширина цели
	/// </summary>
	[Tooltip("Ширина цели")]
	float targetWidth;

	void Awake()
	{
		health = GetComponent<Health>();
		health.SetStartStats();
	}

	void Start()
	{
		SetStats();
		SetStartStats();
		ShowHideMeshs(false);

		NeedToGo = true;
	}

	void Update()
	{
		if (isDied)
			return;

		if (needUpdate)
			SetStats();
		if (currentVortex == null)
			SearchVortex();
		ImproveSkillCooldownCount();

		if (needToGo)
			Move();
		else
		{
			if (target != null)
			{
				if (!target.isActive)
				{
					meleeTarget = null;
					target = null;
				}
			}
			if (target == null)
			{
				if (meleeTarget == null && IsHasTargets())
					target = FindNearesToExitEnemy();
			}

			if (target != null)
				DoActionWithEnemy();
			else
				GoBackToStayPosition();
		}
	}

	void LateUpdate()
	{
		CheckDie();

		if (isDied)
			return;

		//health.SetHealthBar();
		CheckHealth();
	}

	/// <summary>
	/// Поиск свободной вершины
	/// </summary>
	void SearchVortex()
	{
		var vortexs = placePoint.vortexs;
		foreach (var vortex in vortexs)
		{
			WarUnit warrior = vortex.warrior;
			if (warrior == null || warrior != null && warrior.isDied)
			{
				vortex.warrior = this;
				currentVortex = vortex;
				break;
			}
		}
		stayPosition = currentVortex.ownTransform.position;
	}

	/// <summary>
	/// Задать статистики из башни
	/// </summary>
	void SetStats()
	{
		minAttack = tower.minAttack;
		maxAttack = tower.maxAttack;
		health.maxHealth = tower.heath;
		moveSpeed = tower.moveSpeed;
		placePoint = tower.placePoint;
		attackSpeed = tower.attackSpeed;
		needUpdate = false;
		attackSpeedTimer = attackSpeed;
	}

	/// <summary>
	/// Начало смерти
	/// </summary>
	public override void StartDie()
	{
		ClearTarget();
		tower.warriors.Remove(this);
		base.StartDie();
	}

	protected override void Die()
	{
		base.Die();
	}

	/// <summary>
	/// Идти к вершине
	/// </summary>
	void Move()
	{
		if (ownTransform.position != currentVortex.ownTransform.position)
		{
			LookAt(currentVortex.ownTransform.position);
			ownTransform.position = Vector3.MoveTowards(ownTransform.position, currentVortex.ownTransform.position, currentMoveSpeed * Time.deltaTime);
		}
		else
			NeedToGo = false;
	}
}
