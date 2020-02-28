using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        bool logining = Global.Logining("rustamvd92@gmail.com");
        if (!logining)
        {
            Debug.Log("Не получили текущего пользователя :с");
            return;
        }

        Global.GetHavingHeroesTransforms();
        
       
    }

    public void OpenCampaning()
    {
        //SceneManager.LoadScene("Companing");

        
    }
}
