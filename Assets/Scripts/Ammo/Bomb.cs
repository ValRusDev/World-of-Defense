using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : Ammo
{
	/// <summary>
	/// Высота полета снаряда
	/// </summary>
	[Tooltip("Высота полета снаряда")]
	public float height = 10f;

	/// <summary>
	/// Скорость снаряда
	/// </summary>
	[Tooltip("Скорость снаряда снаряда")]
	public float speed = 5.0f;

	/// <summary>
	/// Эффект взрыва
	/// </summary>
	[Tooltip("Эффект взрыва")]
	public ParticleSystem effect;

	/// <summary>
	/// Время жизни взрыва
	/// </summary>
	[Tooltip("Время жизни взрыва")]
	public float lifeTime = 5.0f;
	float lifeTimeTimer = 0f;

	bool needBoom;
	bool isBlownUp;

	float BoomRadius
	{
		get
		{
			BombTower bombTower = tower as BombTower;
			return bombTower.boomRadius;
		}
	}

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

	Vector3 startPosition =>
		tower.ammoSpawnPoint.position;

	Vector3 targetPosition;

	float prevTime;
	float time;

	void Start()
	{
		SetCache();
	}

	void Update()
	{
		CheckRefresh();
		if (!isBlownUp)
		{
			if (!needBoom)
				Move();
			else
				Explosion();
		}
		else
			LifeTimeCheck();
	}

	void CheckRefresh()
	{
		if (!needRefresh)
			return;

		meshRenderer.enabled = true;

		SetTargetPosition();
		needBoom = false;
		isBlownUp = false;
		needRefresh = false;

		time = prevTime = 0;
	}

	void SetTargetPosition()
	{
		if (target == null)
			return;

		Enemy enemy = target as Enemy;
		if (enemy == null)
			return;

		float timeToTarget = 1;// Vector3.Distance(ownTransform.position, target.ownTransform.position) / speed;
		float enemySpeed = enemy.CurrentMoveSpeed;
		Waypoint nextWaypoint = enemy.nextWaypoint;

		targetPosition = Vector3.MoveTowards(target.ownTransform.position, nextWaypoint.ownTransfrom.position, enemySpeed * timeToTarget);
	}

	bool CheckMove()
	{
		return time >= prevTime;
		/*Vector3 direction = Vector3.zero;
		direction = targetPosition - ownTransform.position;
		float magnitude = direction.magnitude;
		return magnitude >= 1.0f;*/
	}

	void LifeTimeCheck()
	{
		if (lifeTimeTimer >= lifeTime)
		{
			effect.Stop();
			effect.gameObject.SetActive(false);

			ReturnToPool();
			lifeTimeTimer = 0;
		}
		else
			lifeTimeTimer += Time.deltaTime;
	}

	void Move()
	{
		//ownTransform.position = Vector3.MoveTowards(ownTransform.position, targetPosition, speed * Time.deltaTime);
		time += Time.deltaTime * speed;
		time %= 5f;

		if (time < prevTime)
		{
			needBoom = true;
			return;
		}

		Vector3 point = MathParabola.Parabola(startPosition, targetPosition, 10, time / 5f);
		ownTransform.position = point;

		prevTime = time;
	}

	void Explosion()
	{
		isBlownUp = true;
		meshRenderer.enabled = false;
		effect.gameObject.SetActive(true);
		effect.Play();

		var colliders = Physics.OverlapSphere(ownTransform.position, BoomRadius);
		var enemies = colliders.Select(c => c.GetComponent<Enemy>()).Where(e => e != null).ToArray();

		short currentDmg = CurrentDmg;
		foreach (var enemy in enemies)
		{
			enemy.health.GetDamage(currentDmg);
		}
	}
}
