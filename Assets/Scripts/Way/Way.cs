using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;
	public List<Waypoint> waypoints = new List<Waypoint>();

	void Awake()
	{
		ownTransform = transform;

		for (int i = 0; i < ownTransform.childCount; i++)
		{
			Transform child = ownTransform.GetChild(i);
			Waypoint waypoint = child.GetComponent<Waypoint>();
			waypoints.Add(waypoint);
		}
	}

	void Start()
    {
        //gameObject.SetActive(false);
    }
}
