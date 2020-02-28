using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Ammo
{
	public float arrowSpeed = 10f;

	Vector3 targetPosition;

	short MinAttack
	{
		get
		{
			return tower.minAttack;
		}
	}

	short MaxAttack
	{
		get
		{
			return tower.maxAttack;
		}
	}

	short CurrentDmg
	{
		get
		{
			return (short)Random.Range(MinAttack, MaxAttack);
		}
	}

	void Start()
	{
		SetCache();
	}

	void Update()
	{
		CheckRefresh();
		Move();
	}

	void CheckRefresh()
	{
		if (!needRefresh)
			return;

		Transform center = target.center;
		if (center != null)
			targetPosition = center.position;
		else
			targetPosition = target.ownTransform.position;

		needRefresh = false;
	}

	void Move()
	{
		if (target != null)
		{
			Transform center = target.center;
			if (center != null)
				targetPosition = center.position;
			else
				targetPosition = target.ownTransform.position;
		}

		//var angle = Vector2.Angle(Vector2.right, target.ownTransform.position - transform.position);//угол между вектором от объекта к мыше и осью х
		//transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < target.ownTransform.position.y ? angle : -angle);//немного магии на последок

		ownTransform.LookAt(targetPosition);
		if (Vector3.Distance(ownTransform.position, targetPosition) > .2f)
			ownTransform.position = Vector3.MoveTowards(ownTransform.position, targetPosition, arrowSpeed * Time.deltaTime);
		else
			Damage();
	}

	void Damage()
	{
		if (target != null)
		{
			// наносим урон
			var health = target.health;
			health.GetDamage(CurrentDmg);
		}

		// убираем снаряд
		ReturnToPool();
	}
}

