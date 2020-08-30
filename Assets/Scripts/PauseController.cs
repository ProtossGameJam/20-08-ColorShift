using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject ClearDontPause, GameOverScreen;
    public Image PauseScreen;
    public Sprite[] RandomTheScreen;

    void Update()
    {
        if(ClearDontPause.activeSelf == false && GameOverScreen.activeSelf == false)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        Cursor.visible = false;

        PauseScreen.sprite = RandomTheScreen[Random.Range(0,RandomTheScreen.Length)];

        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.visible = true;

        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToTheStageSelectMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        GameIsPaused = false;
        SceneManage.instance.SceneLoad("SelectScene");
    }

    public void BackToTheStageMainMenu()
    {
        Time.timeScale = 1f;        
        Cursor.visible = true;
        GameIsPaused = false;
        SceneManage.instance.SceneLoad("MainScene");
    }
}
