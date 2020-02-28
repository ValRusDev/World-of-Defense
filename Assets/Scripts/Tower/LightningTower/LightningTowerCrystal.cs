using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTowerCrystal : MonoBehaviour
{
	Transform ownTransform;
	LightningTower lightningTower;
	Unit target =>
		lightningTower.target;

    void Start()
    {
		ownTransform = transform;
		lightningTower = GetComponentInParent<LightningTower>();
    }

    void Update()
    {
		if (target == null)
			return;

		LookAt(target.ownTransform.position);
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
