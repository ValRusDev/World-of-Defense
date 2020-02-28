using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : PoolObject
{
	public Tower tower;
    public Unit target;
    public AttackingObject attackingObject;

	public bool needRefresh;

    protected Transform ownTransform;
	protected MeshRenderer meshRenderer;

    protected void SetCache()
    {
        ownTransform = transform;
		meshRenderer = GetComponent<MeshRenderer>();
	}
}
