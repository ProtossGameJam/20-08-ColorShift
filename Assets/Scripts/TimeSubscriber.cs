﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeSubscriber : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Text>().text = TimeSender.instance.SecondsString;
    }
}
