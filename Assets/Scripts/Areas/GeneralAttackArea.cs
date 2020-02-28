using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAttackArea : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;

	Transform parent;
	[HideInInspector]
	public AttackingObject attackingObject;

	public bool isRangeAttackArea;
	public bool isMeleeAttackArea;

	protected List<Unit> meleeUnits = new List<Unit>();
	protected List<Unit> rangeUnits = new List<Unit>();


	void Awake()
	{
		ownTransform = transform;
	}

	void Start()
	{
		parent = transform.parent;
		attackingObject = parent.GetComponent<AttackingObject>();
		if (attackingObject == null)
		{
			parent = parent.parent;
			attackingObject = parent.GetComponent<AttackingObject>();
		}
		meleeUnits = attackingObject.meleeUnits;
		rangeUnits = attackingObject.rangeUnits;
	}

	void OnTriggerEnter(Collider other)
	{
		Unit unit = other.GetComponent<Unit>();
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

	void OnTriggerExit(Collider other)
	{
		Unit unit = other.GetComponent<Unit>();
		if (unit == null)
			return;

		if (isRangeAttackArea)
		{
			if (rangeUnits.Contains(unit))
				rangeUnits.Remove(unit);
		}
		if (isMeleeAttackArea)
		{
			if (meleeUnits.Contains(unit))
				meleeUnits.Remove(unit);
		}
	}
}
