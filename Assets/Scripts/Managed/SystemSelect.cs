using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemSelect : MonoBehaviour
{
    public Text hint;
    public string[] hintMsg = {
        "길을 가로막은 장애물",
        "장애물, 기습",
        "색 변화, 통과",
        "선택지, 기습"
    };

    public Image title;
    public Sprite[] titleSpr;

    public AudioClip SelectBgm;
    public AudioClip ClickSfx;

    [SerializeField]
    private GameManage game;
    private SceneManage scene;
    private BgmManage bgm;
    private SfxManage sfx;

    private const int stageMax = 4;
    private int stageNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManage>();
        scene = GameObject.Find("SceneManager").GetComponent<SceneManage>();
        bgm = GameObject.Find("BgmManager").GetComponent<BgmManage>();
        sfx = GameObject.Find("SfxManager").GetComponent<SfxManage>();

        hint = GameObject.Find("HintText").GetComponent<Text>();
        title = GameObject.Find("Stage").GetComponent<Image>();

        bgm.BgmPlay(SelectBgm);

        PageInitialize();

        Cursor.visible = true;
    }

    private void PageInitialize()
    {
        Debug.Log(stageNum);
        hint.text = hintMsg[stageNum - 1];
        title.sprite = titleSpr[stageNum - 1];
    }

    public void onclick_Stage()
    {
        sfx.SfxPlayOnManager(ClickSfx);
        scene.SceneLoad("Stage" + stageNum);
    }
    public void onclick_Left()
    {
        if (this.stageNum > 1) this.stageNum -= 1;

        PageInitialize();
    }
    public void onclick_Right()
    {
        if (this.stageNum < stageMax) this.stageNum += 1;

        PageInitialize();
    }
}