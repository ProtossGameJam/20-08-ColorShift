using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    public GameObject Player;
    public string currentSceneName;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        currentSceneName = SceneManager.GetActiveScene().name;
    }
    void Update()
    {
        if(Player == null)
        {
            Player = null;
        }
    }
}