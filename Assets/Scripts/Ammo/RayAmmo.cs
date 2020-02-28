using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAmmo : Ammo
{
    public Transform spanwPoint;

    private Vector3 scale;
    private Quaternion rotation;

    float currentDmg;

    

    void Update()
    {
        if (target != null)
        {
            Strech();
            Damage();
        }
    }

    void LateUpdate()
    {
        CheckTarget();
        Damage();
    }

    void CheckTarget()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            attackingObject.GetComponent<RayTowerScript>().currentRay = null;
        }
    }

    public void Strech()
    {
        scale = transform.localScale;
        scale.x = Vector2.Distance(spanwPoint.position, target.ownTransform.position) * 2;
        transform.localScale = scale;

        float angle = Mathf.Atan2(target.ownTransform.position.y - spanwPoint.position.y, target.ownTransform.position.x - spanwPoint.position.x);
        rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        transform.rotation = rotation;

        transform.position = (spanwPoint.position + target.ownTransform.position) / 2f;
    }

    void Start()
    {
        spanwPoint = attackingObject.GetComponent<RayTowerScript>().ammoSpawnPoint;
        SetDmg();
    }

    void SetDmg()
    {
        currentDmg = attackingObject.GetComponent<RayTowerScript>().attack;
    }

    void Damage()
    {
        if (target == null)
            return;

        // наносим урон
        /*var healthComponent = target.GetComponent<Enemy>();
        if (healthComponent != null)
            healthComponent.currentHealth -= currentDmg;*/
    }
}
