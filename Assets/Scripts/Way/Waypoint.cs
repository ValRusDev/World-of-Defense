using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransfrom;
	public bool IsStart;
	public bool IsFinish;

	void Awake()
	{
		ownTransfrom = transform;

		SpriteRenderer sr = ownTransfrom.GetComponent<SpriteRenderer>();
		if (sr != null)
			sr.enabled = false;
	}
}
