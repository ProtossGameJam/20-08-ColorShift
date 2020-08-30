using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemLoadingScene : MonoBehaviour
{
    [SerializeField]
    private SceneManage scene;

    void Start()
    {
        scene = GameObject.Find("SceneManager").GetComponent<SceneManage>();
        GameObject.Find("BgmManager").GetComponent<BgmManage>().BgmStop();

        if (scene == null)
        {
            Debug.Log("Exception : Scene Manager is Null.");
            SceneManager.LoadScene("begining");
        }
        else
        {
            StartCoroutine(StartLoading());
        }
    }

    IEnumerator StartLoading ()
    {
        yield return new WaitForSeconds(0.5f);

        string sceneName = scene.NextScene();
        SceneManager.LoadScene(sceneName);
    }
}
