using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BombTower : Tower
{
    public float boomRadius = 3;

    void Update()
    {
        RangeAttack();

		/*if (target != null)
			LookAt(target.ownTransform.position);*/
    }
}
