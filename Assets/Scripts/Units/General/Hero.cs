using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hero : Ally
{
	[Header("Свойства способности")]
	/// <summary>
	/// Кнопка каста скилла
	/// </summary>
	[Tooltip("Кнопка каста скилла")]
	public Transform spellButton;

	/// <summary>
	/// Необходимость задавать цель для скилла
	/// </summary>
	[Tooltip("Необходимость задавать цель для скилла")]
	public bool needTargetForSpell;

	/// <summary>
	/// Точка для скилла
	/// </summary>
	[Tooltip("Точка для скилла")]
	public Vector2 positionForSpell;

	[Header("Свойства героя из БД")]
	public uint id;
	public byte level;
	public uint experience;

	void Awake()
	{
		SetStartStats();
	}

	void Start()
	{
		stayPosition = ownTransform.position;
		ShowHideMeshs(false);
	}

	void Update()
	{
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

	/*void LateUpdate()
	{
		health.SetHealthBar();
	}*/

	/// <summary>
	/// Идти к цели
	/// </summary>
	void Move()
	{
		if (ownTransform.position != stayPosition)
		{
			LookAt(stayPosition);
			ownTransform.position = Vector3.MoveTowards(ownTransform.position, stayPosition, currentMoveSpeed * Time.deltaTime);
		}
		else
			needToGo = false;
	}

	/// <summary>
	/// Каст способности
	/// </summary>
	public virtual void Cast()
	{
		Debug.Log("I am a Hero!");
	}

	/// <summary>
	/// Идкти к точке
	/// </summary>
	/// <param name="position">Точка</param>
	public void GoToPosition(Vector3 position)
	{
		needToGo = true;
		stayPosition = position;
		if (meleeTarget != null)
		{
			meleeTarget.GetComponent<Enemy>().target = null;
			meleeTarget = null;
		}
	}
}
