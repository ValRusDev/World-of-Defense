using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlace : SelectingObject
{
	void Awake()
	{
		ownTransform = transform;
	}

	private void Start()
    {
        ShowHideMeshs(false);
    }
}
