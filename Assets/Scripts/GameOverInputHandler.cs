using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverInputHandler : MonoBehaviour
{
    SceneManage scene;
    GameManage game;
    private void Start()
    {
        scene = GameObject.Find("SceneManager").GetComponent<SceneManage>();
        game = GameObject.Find("GameManager").GetComponent<GameManage>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            scene.SceneLoad("SelectScene");

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
