using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBegining : MonoBehaviour
{
    [SerializeField]
    private GameManage game;
    private SceneManage scene;

    void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManage>();
        scene = GameObject.Find("SceneManager").GetComponent<SceneManage>();

        scene.SceneLoad("MainScene");
    }
}
