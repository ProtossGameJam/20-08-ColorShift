using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage instance;

    [SerializeField]
    private string nextScene = "IntroScene";

    void Awake()
    {
        if (instance != this) instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneLoad(string SceneName)
    {
        this.nextScene = SceneName;

        StartCoroutine(SceneLoader());
    }
    IEnumerator SceneLoader ()
    {
        AsyncOperation AsyncLoading = SceneManager.LoadSceneAsync("LoadingScene");

        while (!AsyncLoading.isDone)
        {
            yield return null;
        }
    }

    public string NextScene()
    {
        return this.nextScene;
    }
}
