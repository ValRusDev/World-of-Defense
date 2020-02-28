using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerAttackArea : GeneralAttackArea
{
	List<Collider> processedColliders = new List<Collider>();

	private void OnTriggerStay(Collider other)
	{
		if (processedColliders.Contains(other))
			return;
		else
			processedColliders.Add(other);

		Transform collisisionTransform = other.transform;
		Unit unit = collisisionTransform.GetComponent<Unit>();
		if (unit == null)
			return;

		if (isRangeAttackArea)
		{
			if (!rangeUnits.Contains(unit))
				rangeUnits.Add(unit);
		}
		if (isMeleeAttackArea)
		{
			if (!meleeUnits.Contains(unit))
				meleeUnits.Add(unit);
		}
	}
}
