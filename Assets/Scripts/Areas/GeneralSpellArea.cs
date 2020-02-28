using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSpellArea : MonoBehaviour
{
    public Transform parent;
    public AttackingObject parentComponent;

    List<Unit> spellUnits = new List<Unit>();

	void Start()
    {
        parent = transform.parent;
        parentComponent = parent.GetComponent<AttackingObject>();
        if (parentComponent == null)
        {
            parent = parent.parent;
            parentComponent = parent.GetComponent<AttackingObject>();
        }
        spellUnits = parentComponent.spellUnits;
    }

	private void OnTriggerEnter(Collider other)
	{
		Unit unit = other.GetComponent<Unit>();
		if (unit == null)
			return;

		if (!spellUnits.Contains(unit))
			spellUnits.Add(unit);
	}

	private void OnTriggerExit(Collider other)
	{
		Transform collisisionTransform = other.transform;
		Unit unit = collisisionTransform.GetComponent<Unit>();
		if (unit == null)
			return;

		if (spellUnits.Contains(unit))
			spellUnits.Remove(unit);
	}
}
