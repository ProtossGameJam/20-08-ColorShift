using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManage : MonoBehaviour
{
    public static SfxManage instance;
    public AudioSource managerSrc;
    [SerializeField]
    private float volume = 0.5f;

    void Awake() {
        instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        managerSrc = this.gameObject.GetComponent<AudioSource>();
        managerSrc.volume = this.volume;
    }
    public void SetVolume (float vol)
    {
        this.volume = vol;
        managerSrc.volume = this.volume;
    }
    public float getVolume ()
    {
        return this.volume;
    }
    public void SfxPlay(AudioSource src, AudioClip clip)
    {
        if (src == null) return;

        src.volume = volume;
        src.clip = clip;
        src.Play();
    }

    public void SfxPlayOnManager (AudioClip clip)
    {
        managerSrc.clip = clip;
        managerSrc.Play();
    }
}
