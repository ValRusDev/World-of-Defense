using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePoint : MonoBehaviour
{
    public VortexScript[] vortexs = new VortexScript[3];

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            vortexs[i] = transform.GetChild(i).GetComponent<VortexScript>();
        }
    }
}
