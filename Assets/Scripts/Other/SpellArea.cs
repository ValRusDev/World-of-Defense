using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellArea : MonoBehaviour
{
	public Transform parent;
	AttackingObject attackingObject;

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
		innerUnits = attackingObject.spellUnits;

		if (!innerUnits.Contains(unit))
			innerUnits.Add(unit);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Unit unit = collision.GetComponent<Unit>();
		if (unit == null)
			return;

		List<Unit> innerUnits = new List<Unit>();
		innerUnits = attackingObject.spellUnits;

		if (innerUnits.Contains(unit))
			innerUnits.Remove(unit);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Unit unit = collision.GetComponent<Unit>();
		if (unit == null)
			return;

		List<Unit> innerUnits = new List<Unit>();
		innerUnits = attackingObject.spellUnits;

		if (!innerUnits.Contains(unit))
			innerUnits.Add(unit);
	}
}
