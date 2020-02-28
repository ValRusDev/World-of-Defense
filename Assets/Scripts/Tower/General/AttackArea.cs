using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
	public Transform parent;
	AttackingObject attackingObject;

	public bool isRangeAttackArea;
	public bool isMeleeAttackArea;

	void Start()
	{
		parent = transform.parent;
		attackingObject = parent.GetComponent<AttackingObject>();
		if (attackingObject == null)
		{
			parent = parent.parent;
			attackingObject = parent.GetComponent<AttackingObject>();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Unit unit = collision.GetComponent<Unit>();
		if (unit == null)
			return;

		List<Unit> innerUnits = new List<Unit>();
		if (isRangeAttackArea)
			innerUnits = attackingObject.rangeUnits;
		else if (isMeleeAttackArea)
			innerUnits = attackingObject.meleeUnits;

		if (!innerUnits.Contains(unit))
			innerUnits.Add(unit);

		/*if (!innerUnits.Contains(collsisionTransform) && collsisionTransform.GetComponent<Unit>() != null)
            innerUnits.Add(collsisionTransform);
        if (isRangeAttackArea)
            attackingObject.rangeInnerTransforms = innerUnits;
        if (isMeleeAttackArea)
            attackingObject.meleeInnerTransforms = innerUnits;*/
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Unit unit = collision.GetComponent<Unit>();
		if (unit == null)
			return;

		List<Unit> innerUnits = new List<Unit>();
		if (isRangeAttackArea)
			innerUnits = attackingObject.rangeUnits;
		else if (isMeleeAttackArea)
			innerUnits = attackingObject.meleeUnits;

		if (innerUnits.Contains(unit))
			innerUnits.Remove(unit);
		/*if (innerTransofrms.Contains(collsisionTransform))
            innerTransofrms.Remove(collsisionTransform);
        if (isRangeAttackArea)
            attackingObject.rangeInnerTransforms = innerTransofrms;
        if (isMeleeAttackArea)
            attackingObject.meleeInnerTransforms = innerTransofrms;*/
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Unit unit = collision.GetComponent<Unit>();
		if (unit == null)
			return;

		List<Unit> innerUnits = new List<Unit>();
		if (isRangeAttackArea)
			innerUnits = attackingObject.rangeUnits;
		if (isMeleeAttackArea)
			innerUnits = attackingObject.meleeUnits;

		if (!innerUnits.Contains(unit))
			innerUnits.Add(unit);
	}
}
