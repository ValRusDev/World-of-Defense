using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveButtonCanvas : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;
	MainCamera mainCamera;

	void Awake()
	{
		ownTransform = transform;
		mainCamera = Camera.main.GetComponent<MainCamera>();
	}

	void Update()
    {
		ownTransform.LookAt(ownTransform.position + mainCamera.ownTransform.rotation * Vector3.back, 
			mainCamera.ownTransform.rotation * Vector3.up);
    }
}
