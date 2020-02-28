using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Unit : AttackingObject
{
	/// <summary>
	/// Целя для ближней атаки
	/// </summary>
	[Tooltip("Целя для ближней атаки")]
	public Unit meleeTarget;

	[Header("Передвижение")]
	public PlacePoint placePoint;

	/// <summary>
	/// Позиция рядом с врагом
	/// </summary>
	[Tooltip("Позиция рядом с врагом")]
	public Vector3 stayNearEnemyPosition;

	/// <summary>
	/// Скорость передвижения
	/// </summary>
	[Tooltip("Скорость передвижения")]
	public float moveSpeed = 1f;

	/// <summary>
	/// Текущая скорость передвижения
	/// </summary>
	[Tooltip("Текущая скорость передвижения")]
	public float currentMoveSpeed;
	public float CurrentMoveSpeed
	{
		get
		{
			return currentMoveSpeed;
		}
		set
		{
			currentMoveSpeed = value * speedMultiplier;

			if (animator != null)
			{
				animator.SetFloat("CurrentMoveSpeed", currentMoveSpeed);
				if (currentMoveSpeed != 0)
				{
					if (animator.GetBool("IsAttacking"))
						animator.SetBool("IsAttacking", false);
				}
			}
		}
	}

	/// <summary>
	/// Летает или нет
	/// </summary>
	[Tooltip("Летает или нет")]
	public bool fly = false;

	/// <summary>
	/// Здоровье
	/// </summary>
	[HideInInspector]
	public Health health;

	[Header("Поведение трупа"), Header("Время жизни трупа"), SerializeField]
	protected float corpseLifeTime = 3f;
	protected float corpseLifeTimeTimer = 0;
	protected bool isDied;

	[Header("Сопротивления к урону")]
	/// <summary>
	/// Сопротивление к физическому урону
	/// </summary>
	[Tooltip("Сопротивление к физическому урону")]
	public byte phisDef = 0;

	/// <summary>
	/// Сопротивление к магическуму урону
	/// </summary>
	[Tooltip("Сопротивление к магическуму урону")]
	public byte magicDef = 0;

	[Header("Реверсивное движение")]
	/// <summary>
	/// Реверс движения или нет
	/// </summary>
	[Tooltip("Реверс движения или нет")]
	public bool reverseMove;

	/// <summary>
	/// Время реверсивного движения
	/// </summary>
	[Tooltip("Время реверсивного движения")]
	protected float reverseTime;

	/// <summary>
	/// Время реверсивного движения
	/// </summary>
	[Tooltip("Время реверсивного движения")]
	protected float reverseCurrentTime;

	/// <summary>
	/// Спрайт юнита
	/// </summary>
	[Header("Свойства юнита"), Tooltip("Спрайт юнита")]
	public SpriteRenderer spriteRenderer;

	/// <summary>
	/// Центр модели
	/// </summary>
	[Tooltip("Центр модели")]
	public Transform center;

	/// <summary>
	/// Задать начальные свойства
	/// </summary>
	public void SetStartStats()
	{
		ownTransform = transform;
		ownCollider = GetComponent<Collider2D>();
		//ownMeshRenderer = GetComponentInChildren<MeshRenderer>();
		CurrentMoveSpeed = moveSpeed;
		skillCooldownCount = skillCooldown;
		attackSpeedTimer = attackSpeed;

		health = GetComponent<Health>();
		health.SetStartStats();

		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		if (animator != null)
			animator.SetFloat("CurrentMoveSpeed", currentMoveSpeed);
	}

	/// <summary>
	/// Ближняя атака
	/// </summary>
	public void MeleeAttack()
	{
		CurrentMoveSpeed = 0;

		LookAt(target.ownTransform.position);
		if (attackSpeedTimer >= attackSpeed)
		{
			if (target != null)
			{
				if (animator != null)
					animator.SetBool("IsAttacking", true);
				else
					DoHit();
			}
			attackSpeedTimer = 0;
		}
		else
			attackSpeedTimer += Time.deltaTime;
	}

	/// <summary>
	/// Нанести удар
	/// </summary>
	public void DoHit()
	{
		if (target != null)
		{
			SetCurrentDmg();
			// наносим урон
			target.health.GetDamage(currentDmg);
		}

		attackSpeedTimer = 0;
	}

	/// <summary>
	/// Задать средний урон
	/// </summary>
	void SetCurrentDmg()
	{
		currentDmg = (short)Random.Range(minAttack, maxAttack);
	}

	/// <summary>
	/// Найти позицию рядом с врагом
	/// </summary>
	public void SetNearEnemyPosition()
	{
		/*bool left = ownTransform.position.x < target.ownTransform.position.x;
        var targetSpriteRenderer = target.spriteRenderer;
        var targetWidth = targetSpriteRenderer.bounds.size.x;
        var unitWidth = spriteRenderer.bounds.size.x;
        var halfWidth = targetWidth / 2 + unitWidth / 2;
        var standPlace = target.ownTransform.position.x - halfWidth;
        if (!left)
            standPlace = target.ownTransform.position.x + halfWidth;
        stayNearEnemyPosition = new Vector2(standPlace, target.ownTransform.position.y);*/

		float distance = 2.0f;
		if (ownMeshRenderer != null && target.ownMeshRenderer != null)
			distance = ownMeshRenderer.bounds.size.x / 2 + target.ownMeshRenderer.bounds.size.x / 2;

		stayNearEnemyPosition = Vector3.MoveTowards(target.ownTransform.position, ownTransform.position, Vector3.Distance(ownTransform.position, target.ownTransform.position) - distance);
	}

	/// <summary>
	/// Находится ли рядом с противником
	/// </summary>
	/// <returns></returns>
	public virtual bool IsNearEnemy()
	{
		float minDistane = 2.0f;
		if (ownMeshRenderer != null && target.ownMeshRenderer != null)
			minDistane = ownMeshRenderer.bounds.size.x / 2 + target.ownMeshRenderer.bounds.size.x / 2;

		float distance = Vector3.Distance(ownTransform.position, target.ownTransform.position);
		return distance < minDistane;
		//return ownTransform.position == stayNearEnemyPosition;
	}

	/// <summary>
	/// Сбросить счетчики скиллов
	/// </summary>
	public void ResetSkillsCounters()
	{
		skillCooldownCount = skillCooldown;
	}

	/// <summary>
	/// Проверка здоровья юнита. 
	/// Если меньше нуля, то умираем
	/// </summary>
	protected void CheckHealth()
	{
		if (health.CurrentHealth > 0)
			return;

		if (animator != null)
			animator.SetBool("IsDied", true);
	}

	/// <summary>
	/// Провека трупа (скрывать его или нет)
	/// </summary>
	protected void CheckDie()
	{
		if (!isDied)
			return;

		if (corpseLifeTimeTimer >= corpseLifeTime)
			Die();
		else
			corpseLifeTimeTimer += Time.deltaTime;
	}

	public virtual void StartDie()
	{
		isActive = false;
		isDied = true;
	}

	/// <summary>
	/// Смерть
	/// </summary>
	protected virtual void Die()
	{
		gameObject.SetActive(false);
	}
}
