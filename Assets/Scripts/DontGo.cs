using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontGo : MonoBehaviour
{
    public Transform Back;
    public Transform BackCamera;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().transform.position = Back.position;
            CameraController.instance.target = BackCamera;
        }
    }
}
