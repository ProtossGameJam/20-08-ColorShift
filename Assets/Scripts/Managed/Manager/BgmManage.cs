using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManage : MonoBehaviour
{
    [SerializeField]
    private AudioSource src;

    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        src = this.gameObject.GetComponent<AudioSource>();

        src.volume = 0.1f;
    }
    public void BgmPlay(AudioClip clip)
    {
        src.clip = clip;

        src.Play();
    }
    public void BgmStop()
    {
        if (src.isPlaying == true)
        {
            Debug.Log("Bgm has Stopped.");
            src.Stop();
        }
    }
    public void BgmVolume(float vol)
    {
        if (vol >= 0 && vol <= 1)
        {
            src.volume = vol;
        }
    }
}