using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMainScene : MonoBehaviour
{
    public AudioClip MainBgm;
    public AudioClip ClickSfx;

    [SerializeField]
    private GameManage game;
    private SceneManage scene;
    private BgmManage bgm;
    private SfxManage sfx;
    private AudioSource src;

    void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManage>();
        scene = GameObject.Find("SceneManager").GetComponent<SceneManage>();
        bgm = GameObject.Find("BgmManager").GetComponent<BgmManage>();
        sfx = GameObject.Find("SfxManager").GetComponent<SfxManage>();
        src = this.gameObject.GetComponent<AudioSource>();

        bgm.BgmPlay(MainBgm); 
    }

    // Update is called once per frame
    public void onclick_StartBtn()
    {
        Debug.Log("Start Button Selected.");
        sfx.SfxPlayOnManager(ClickSfx);
        scene.SceneLoad("SelectScene");
    }
    public void onclick_SettingBtn()
    {
        Debug.Log("Select Button Selected.");
        sfx.SfxPlayOnManager(ClickSfx);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}