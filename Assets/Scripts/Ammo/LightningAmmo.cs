using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAmmo : Ammo
{
	/// <summary>
	/// Время жизни
	/// </summary>
	[Header("Время жизни"), Tooltip("Время жизни")]
	public float lifeTime;

	/// <summary>
	/// Таймер времени жизни
	/// </summary>
	[Tooltip("Таймер времени жизни")]
	public float lifeTimeTimer;

	bool causedDamage;

	LightningBoltScript lightningBoltScript;

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

	void Awake()
	{
		lightningBoltScript = GetComponent<LightningBoltScript>();
	}

	void Start()
	{
		SetCache();
	}

	void Update()
	{
		CheckRefresh();
		LifeTimeCheck();
		ToDamage();
	}

	void CheckRefresh()
	{
		if (!needRefresh)
			return;

		causedDamage = false;

		needRefresh = false;
	}

	void LifeTimeCheck()
	{
		if (lifeTimeTimer >= lifeTime)
		{
			ReturnToPool();
			lifeTimeTimer = 0;
		}
		else
			lifeTimeTimer += Time.deltaTime;
	}

	void ToDamage()
	{
		if (causedDamage)
			return;

		target.health.GetDamage(CurrentDmg);
		causedDamage = true;
	}
}
