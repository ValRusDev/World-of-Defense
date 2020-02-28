using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexScript : MonoBehaviour
{
	public Transform ownTransform;
    public WarUnit warrior;

	void Awake()
	{
		ownTransform = transform;
	}
}
