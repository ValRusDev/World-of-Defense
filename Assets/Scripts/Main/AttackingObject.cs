using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackingObject : SelectingObject
{
	/// <summary>
	/// Множитель скорости
	/// </summary>
	[Header("Множитель скорости"), Tooltip("Множитель скорости"), SerializeField]
	protected float speedMultiplier = 1;
	public float SpeedMultiplier
	{
		get
		{
			return speedMultiplier;
		}
		set
		{
			speedMultiplier = value;
		}
	}

	[Header("Радиусы")]
	/// <summary>
	/// Радиус дальней атаки
	/// </summary>
	[Tooltip("Радиус дальней атаки")]
	public GeneralAttackArea rangeAttackArea;

	/// <summary>
	/// Радиус ближней атаки
	/// </summary>
	[Tooltip("Радиус ближней атаки")]
	public GeneralAttackArea meleeAttackArea;

	/// <summary>
	/// Радиус магической атаки
	/// </summary>
	[Tooltip("Радиус магической атаки")]
	public GeneralSpellArea spellArea;

	[Header("Юниты")]
	/// <summary>
	/// Юниты, попавшие в радиус ближней атаки
	/// </summary>
	[Tooltip("Юниты, попавшие в радиус ближней атаки")]
	public List<Unit> meleeUnits = new List<Unit>();

	/// <summary>
	/// Юниты, попавшие в радиус дальней атаки
	/// </summary>
	[Tooltip("Юниты, попавшие в радиус дальней атаки")]
	public List<Unit> rangeUnits = new List<Unit>();

	/// <summary>
	/// Юниты, попавшие в радиус магической атаки
	/// </summary>
	[Tooltip("Юниты, попавшие в радиус магической атаки")]
	public List<Unit> spellUnits = new List<Unit>();

	/// <summary>
	/// Доступные враги
	/// </summary>
	[Tooltip("Доступные враги")]
	public List<Unit> availableEnemies = new List<Unit>();

	/// <summary>
	/// Цель для атаки
	/// </summary>
	[Tooltip("Цель для атаки")]
	public Unit target;

	[Header("Параметры атаки")]
	/// <summary>
	/// Минимальная атака
	/// </summary>
	[Tooltip("Минимальная атака")]
	public short minAttack;

	/// <summary>
	/// Максимальная атака
	/// </summary>
	[Tooltip("Максимальная атака")]
	public short maxAttack;

	/// <summary>
	/// Текущая атака
	/// </summary>
	[Tooltip("Текущая атака")]
	public short currentDmg;

	/// <summary>
	/// Урон от магии
	/// </summary>
	[Tooltip("Урон от магии")]
	public short spellDmg;

	[Header("Таймеры атаки")]
	/// <summary>
	/// Скорость атаки
	/// </summary>
	[Tooltip("Скорость атаки")]
	public float attackSpeed;

	/// <summary>
	/// Счетчик для таймера времени атаки
	/// </summary>
	[Tooltip("Счетчик для таймера времени атаки")]
	public float attackSpeedTimer;

	/// <summary>
	/// Кулдаун скилла
	/// </summary>
	[Tooltip("Кулдаун скилла")]
	public float skillCooldown;

	/// <summary>
	/// Счетчик для таймера кулдауна скилла
	/// </summary>
	[Tooltip("Счетчик для таймера кулдауна скилла")]
	public float skillCooldownCount;

	[Header("Настройки способности атаковать")]
	/// <summary>
	/// Возможность атаковать наземных юнитов
	/// </summary>
	[Tooltip("Возможность атаковать наземных юнитов")]
	public bool canAttackGroundUnit;

	/// <summary>
	/// Возможность атаковать воздушных юнитов
	/// </summary>
	[Tooltip("Возможность атаковать воздушных юнитов")]
	public bool canAttackFlyUnit;

	[Header("Снаряды для выстрела")]
	/// <summary>
	/// Префаб снаряда
	/// </summary>
	[Tooltip("Префаб снаряда")]
	public Ammo ammo;

	/// <summary>
	/// Точка появления снарядов
	/// </summary>
	[Tooltip("Точка появления снарядов")]
	public Transform ammoSpawnPoint;

	/// <summary>
	/// Активный или нет
	/// </summary>
	[Tooltip("Активный или нет")]
	public bool isActive;

	protected Animator animator;

	/// <summary>
	/// Поиск ближайшего к выходу врага
	/// </summary>
	/// <returns></returns>
	protected Unit FindNearesToExitEnemy()
	{
		// получаем доступные цели
		availableEnemies = GetAvailableEnemies();
		// получаем врагов
		List<Enemy> enemies = GetEnemies();
		// находим ближайшего из врагов
		Unit toAttackObject = GetTargetFromEnemies(enemies);
		return toAttackObject;
	}

	/// <summary>
	/// Получить цель из доступных врагов
	/// </summary>
	/// <param name="enemies">Список врагов</param>
	/// <returns></returns>
	Unit GetTargetFromEnemies(List<Enemy> enemies)
	{
		Unit toAttackObject = null;
		float minDistance = 5000;
		foreach (var enemy in enemies)
		{
			Way way = enemy.way;
			//Transform currentWaypoint = enemy.currentWaypoint;
			Waypoint nextWaypoint = enemy.nextWaypoint;
			if (nextWaypoint != null)
			{
				float distance = (enemy.ownTransform.position - nextWaypoint.ownTransfrom.position).sqrMagnitude;
				var lastWaypoint = way.ownTransform.GetChild(way.ownTransform.childCount - 1);
				if (nextWaypoint != lastWaypoint)
				{
					bool startCount = false;
					for (int i = 0; i < way.ownTransform.childCount - 1; i++)
					{
						var waypoint = way.ownTransform.GetChild(i);
						if (waypoint == nextWaypoint)
							startCount = true;
						if (startCount)
						{
							var nextPoint = way.ownTransform.GetChild(i + 1);
							if (nextPoint != null)
							{
								distance += (waypoint.position - nextPoint.position).magnitude;
							}
						}
					}
				}
				if (distance < minDistance)
				{
					minDistance = distance;
					toAttackObject = enemy;
				}
			}
		}
		return toAttackObject;
	}

	/// <summary>
	/// Получить врагов их доступных целей
	/// </summary>
	/// <returns></returns>
	List<Enemy> GetEnemies()
	{
		List<Enemy> allEnemies = availableEnemies.OfType<Enemy>().Where(e => e.isActive).ToList();
		// сначала ищем неатакованных
		List<Enemy> freeEnemies = allEnemies.Where(e => e.target == null).ToList();
		if (freeEnemies.Count > 0)
			allEnemies = freeEnemies;

		// в первую очередь атакуем ближайших
		List<Enemy> enemies = allEnemies.Where(e => meleeUnits.Contains(e)).ToList();
		if (enemies.Count > 0)
		{
			// ищем летающих
			List<Enemy> flyEnemies = enemies.Where(e => e.fly).ToList();
			if (flyEnemies.Count > 0)
				enemies = flyEnemies;
		}
		// если нет ближайших, то атакуем дальних
		else
		{
			// ищем летающих
			List<Enemy> flyEnemies = allEnemies.Where(e => e.fly).ToList();
			// если не нашли, то ищем наземных
			if (flyEnemies.Count != 0)
				enemies = flyEnemies;
			else
				enemies = allEnemies;
		}
		return enemies;
	}

	/// <summary>
	/// Получить доступных врагов
	/// </summary>
	/// <returns></returns>
	List<Unit> GetAvailableEnemies()
	{
		availableEnemies = new List<Unit>();
		if (meleeAttackArea != null)
		{
			foreach (var unit in meleeUnits)
			{
				if (unit == null)
					continue;

				if (!unit.isActive)
					continue;

				if (unit.reverseMove)
					continue;

				if (canAttackFlyUnit)
				{
					if (unit.fly)
						availableEnemies.Add(unit);
				}
				if (canAttackGroundUnit)
				{
					if (!unit.fly)
						availableEnemies.Add(unit);
				}
			}
		}
		if (rangeAttackArea != null)
		{
			foreach (var unit in rangeUnits)
			{
				if (unit == null)
					continue;

				if (!unit.isActive)
					continue;

				/*if (unit.reverseMove)
					continue;*/

				if (canAttackFlyUnit)
				{
					if (unit.fly)
						availableEnemies.Add(unit);
				}
				if (canAttackGroundUnit)
				{
					if (!unit.fly)
						availableEnemies.Add(unit);
				}
			}
		}

		return availableEnemies;
	}

	/// <summary>
	/// Дальняя атака
	/// </summary>
	protected void RangeAttack()
	{
		if (rangeUnits.Count == 0)
		{
			attackSpeedTimer = 0;
			return;
		}

		if (target != null)
		{
			if (!target.isActive)
				target = null;
		}
		if (target == null)
			target = FindNearesToExitEnemy();

		if (target == null)
		{
			attackSpeedTimer = 0;
			return;
		}

		if (attackSpeedTimer >= attackSpeed)
		{
			Shoot();
			target = null;
			attackSpeedTimer = 0;
		}
		else
			attackSpeedTimer += Time.deltaTime;
	}

	// выстрел
	protected virtual void Shoot()
	{
		Transform ammoPoolObject = PoolManager.GetObject(this.ammo.name, ammoSpawnPoint.position, Quaternion.identity);
		Ammo ammo = ammoPoolObject.GetComponent<Ammo>();
		ammo.target = target;
		ammo.attackingObject = this;
		ammo.tower = this as Tower;
		ammo.needRefresh = true;
	}

	/// <summary>
	/// Увеличить счетчик кулдауна скилла
	/// </summary>
	public void ImproveSkillCooldownCount()
	{
		if (skillCooldownCount < skillCooldown)
			skillCooldownCount += Time.deltaTime;
	}

	/// <summary>
	/// Сбросить счетчик кулдауна скилла
	/// </summary
	public void SetToZeroSkillCooldownCount()
	{
		skillCooldownCount = 0;
	}

	/// <summary>
	/// Окончание атаки
	/// </summary>
	public void EndAttack()
	{
		if (animator != null)
			animator.SetBool("IsAttacking", false);
	}

	/// <summary>
	/// Смотреть на объект
	/// </summary>
	/// <param name="position">Позиция объекта</param>
	protected void LookAt(Vector3 position)
	{
		Vector3 targetPostition = new Vector3(position.x,
							ownTransform.position.y,
							position.z);
		ownTransform.LookAt(targetPostition);
	}
}
