using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ally : Unit
{
	[Header("Настойки союзников")]
	/// <summary>
	/// Точка "патруля"
	/// </summary>
	[Tooltip("Точка \"патруля\"")]
	public Vector3 stayPosition;

	/// <summary>
	/// Нужно идти или нет
	/// </summary>
	[Tooltip("Нужно идти или нет")]
	protected bool needToGo;

	public bool NeedToGo
	{
		get
		{
			return needToGo;
		}
		set
		{
			needToGo = value;
			if (!needToGo)
				CurrentMoveSpeed = 0;
			else
			{
				ClearTarget();
				CurrentMoveSpeed = moveSpeed;
			}
		}
	}

	/// <summary>
	/// Совершить действия с врагом
	/// </summary>
	public void DoActionWithEnemy()
	{
		SetNearEnemyPosition();
		var enemyComponent = target.GetComponent<Enemy>();
		bool enemyFly = enemyComponent.fly;
		// если герой НЕ летает
		if (!fly)
		{
			// если враг летает
			if (enemyFly)
				RangeAttack();
			// если враг НЕ летает
			else
				ActionsForSameClasses();
		}
		// если союзник летает
		else
		{
			// если враг летает
			if (!enemyFly)
				ActionsForSameClasses();
			// если враге НЕ летает
			else
				RangeAttack();
		}
	}

	/// <summary>
	/// Действия для "одинаковых классов"
	/// </summary>
	void ActionsForSameClasses()
	{
		// если враг в радиусе ближней атаки
		if (meleeUnits.Contains(target))
		{
			// если союзник возле врага
			if (IsNearEnemy())
				MeleeAttack();
			else
				GoToEnemy();
		}
		// если враг в радисуе дальней атаки
		else if (rangeUnits.Contains(target))
			RangeAttack();
	}

	/// <summary>
	/// Идти к врагу
	/// </summary>
	public void GoToEnemy()
	{
		CurrentMoveSpeed = moveSpeed;
		if (meleeTarget == null)
			meleeTarget = target;
		target.target = this;

		LookAt(target.ownTransform.position);
		ownTransform.position = Vector3.MoveTowards(ownTransform.position, target.ownTransform.position, currentMoveSpeed * Time.deltaTime);
		FreezeAttackAreas();
	}

	/// <summary>
	/// Вернуться к точке "патруля"
	/// </summary>
	public void GoBackToStayPosition()
	{
		if (ownTransform.position != stayPosition)
		{
			CurrentMoveSpeed = moveSpeed;

			LookAt(stayPosition);
			ownTransform.position = Vector3.MoveTowards(ownTransform.position, stayPosition, currentMoveSpeed * Time.deltaTime);
		}
		else if (CurrentMoveSpeed != 0)
			CurrentMoveSpeed = 0;
		FreezeAttackAreas();
	}

	/// <summary>
	/// "Заморозить" радиусы атаки
	/// </summary>
	void FreezeAttackAreas()
	{
		if (rangeAttackArea != null)
			rangeAttackArea.ownTransform.position = stayPosition;
		if (meleeAttackArea != null)
			meleeAttackArea.ownTransform.position = stayPosition;
	}

	/// <summary>
	/// Имеет цели
	/// </summary>
	/// <returns></returns>
	public bool IsHasTargets()
	{
		bool haveTargets = false;
		var allUnits = meleeUnits.Concat(rangeUnits);
		var enemies = allUnits.Where(t => t.isActive && (t as Enemy) != null);
		haveTargets = enemies.Count() > 0;

		return haveTargets;
	}

	/// <summary>
	/// Очистить цели у себя и врагов
	/// </summary>
	public void ClearTarget()
	{
		target = null;
		meleeTarget = null;
		WarUnit warUnit = this as WarUnit;
		var allUnits = meleeUnits.Concat(rangeUnits);
		Unit[] enemies = new Unit[0];
		if (warUnit != null)
		{
			WarTower warTower = warUnit.tower;
			var warriors = warTower.warriors;
			enemies = allUnits.Where(u => warriors.Contains(u.target) || warriors.Contains(u.meleeTarget)).ToArray();
		}
		else
			enemies = allUnits.Where(u => u.target == this || u.meleeTarget == this).ToArray();

		if (enemies.Length == 0)
			return;

		foreach (var enemy in enemies)
		{
			enemy.target = null;
			enemy.meleeTarget = null;
		}
	}
}
