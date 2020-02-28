using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTowerScript : Tower
{
    public float attack = .1f;
    public Transform ray;
    public Transform currentRay;

    private void Start()
    {
    }

    private void Update()
    {
        if (rangeUnits.Count == 0)
            return;
        if (currentRay != null)
            return;

        //GameObject toAttackObject = null;
        float minDistance = 5000;
        foreach (var innerCollider in rangeUnits)
        {
            if (innerCollider == null)
                continue;
            GameObject obj = innerCollider.gameObject;
            var objPosition = obj.transform.position;
            float distance = (transform.position - objPosition).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                //toAttackObject = obj;
            }
        }
    }

    void AttackObject(GameObject toAttackObject, Transform spawnPoint, Transform arrow)
    {
        Transform rayInst = Instantiate(arrow, spawnPoint.transform.position, Quaternion.identity);
        var targetPosition = toAttackObject.transform;
        rayInst.GetComponent<RayAmmo>().attackingObject = this;
        rayInst.GetComponent<RayAmmo>().target = toAttackObject.transform.GetComponent<Unit>();
        currentRay = rayInst;
    }
}
