using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArea : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;

	void Awake()
	{
		ownTransform = transform;
	}
}
