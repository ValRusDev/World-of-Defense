using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADSButton : MonoBehaviour
{
    void Start()
    {
        Button button = transform.GetComponent<Button>();
        button.onClick.AddListener(ShowADS);
    }

    void ShowADS()
    {
        Global.ShowRewardedVideo();
    }
}
