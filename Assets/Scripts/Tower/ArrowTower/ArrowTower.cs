using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower
{
	/// <summary>
	/// Лучники
	/// </summary>
	[Header("Лучники"), Tooltip("Лучники")]
	public List<TowerArcher> archers = new List<TowerArcher>();

    void Update()
    {
        RangeAttack();
    }

	protected override void Shoot()
	{
		foreach (var archer in archers)
			archer.Shoot();
	}
}
