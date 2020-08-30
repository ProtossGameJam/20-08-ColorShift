using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TimeSender : MonoBehaviour
{
    public static TimeSender instance;

    const string backPrefix = "s";

    public string SecondsString => Mathf.FloorToInt(Time.realtimeSinceStartup - startTime).ToString() + backPrefix;

    float startTime;

    

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        startTime = Time.realtimeSinceStartup;
    }

}
