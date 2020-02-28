using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningTower : Tower
{
	/// <summary>
	/// Радиус молнии
	/// </summary>
	[Header("Радиус молнии"), Tooltip("Радиус молнии")]
	public float lightningRadius = 5f;

	List<Enemy> processedEnemies;

	void Update()
	{
		RangeAttack();
	}

	protected override void Shoot()
	{
		processedEnemies = new List<Enemy>();

		CreateLightning(target as Enemy, ammoSpawnPoint.gameObject, target.ownTransform.gameObject);
		processedEnemies.Add(target as Enemy);

		AttackNearEnemies(target as Enemy);
	}

	/// <summary>
	/// Атаковать ближайших врагов
	/// </summary>
	/// <param name="enemy">Враг</param>
	void AttackNearEnemies(Enemy enemy)
	{
		var colliders = Physics.OverlapSphere(enemy.ownTransform.position, lightningRadius);
		var radiusEnemies = colliders.Select(c => c.GetComponent<Enemy>())
			.Where(e => e != null &&
			!processedEnemies.Contains(e) &&
			e.isActive).ToArray();

		foreach (var radiusEnemy in radiusEnemies)
		{
			CreateLightning(radiusEnemy, enemy.center.gameObject, radiusEnemy.center.gameObject);
			processedEnemies.Add(radiusEnemy);

			AttackNearEnemies(radiusEnemy);
		}
	}

	/// <summary>
	/// Создать молнию
	/// </summary>
	/// <param name="enemy">Враг</param>
	/// <param name="startObject">Начальный объект</param>
	/// <param name="endObject">Конечный объект</param>
	void CreateLightning(Enemy enemy, GameObject startObject, GameObject endObject)
	{
		Transform ammoPoolObject = PoolManager.GetObject(ammo.name, Vector3.zero, Quaternion.identity);
		LightningAmmo lightningAmmo = ammoPoolObject.GetComponent<LightningAmmo>();
		lightningAmmo.target = enemy;
		lightningAmmo.attackingObject = this;
		lightningAmmo.tower = this as Tower;
		lightningAmmo.needRefresh = true;

		LightningBoltScript lightningBoltScript = ammoPoolObject.GetComponent<LightningBoltScript>();
		lightningBoltScript.StartObject = startObject;
		lightningBoltScript.EndObject = endObject;
	}
}
