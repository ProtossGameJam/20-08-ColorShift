using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class KillCountSubscriber : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Text>().text = KillCountSender.instance.CountString;
    }
}
