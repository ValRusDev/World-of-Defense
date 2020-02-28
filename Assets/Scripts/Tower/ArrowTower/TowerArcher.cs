using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArcher : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;
	public Transform ammoSpawnPoint;

	ArrowTower arrowTower;
	Arrow arrow =>
		arrowTower.ammo as Arrow;

	Unit target =>
		arrowTower.target;

	void Start()
	{
		ownTransform = transform;
		arrowTower = GetComponentInParent<ArrowTower>();
	}

	void Update()
	{
		if (target == null)
			return;

		LookAt(target.ownTransform.position);
	}

	/// <summary>
	/// Выстрел
	/// </summary>
	public void Shoot()
	{
		Transform ammoPoolObject = PoolManager.GetObject(this.arrow.name, ammoSpawnPoint.position, Quaternion.identity);
		Arrow arrow = ammoPoolObject.GetComponent<Arrow>();
		arrow.target = target;
		arrow.attackingObject = arrowTower;
		arrow.tower = arrowTower;
		arrow.needRefresh = true;
	}

	/// <summary>
	/// Смотреть на объект
	/// </summary>
	/// <param name="position">Позиция объекта</param>
	void LookAt(Vector3 position)
	{
		Vector3 targetPostition = new Vector3(position.x,
							ownTransform.position.y,
							position.z);
		ownTransform.LookAt(targetPostition);
	}
}
