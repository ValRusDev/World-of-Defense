using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subwave : MonoBehaviour
{
    public List<Transform> unitsPacks;
    public Button startButton;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            var unitPacksComponent = child.GetComponent<UnitsPack>();
            if (unitPacksComponent != null)
                unitsPacks.Add(child);
        }
    }
}
