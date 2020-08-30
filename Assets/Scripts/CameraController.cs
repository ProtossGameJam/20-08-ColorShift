using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;
    void Awake() {
        instance = this;
    }

    void Update()
    {
        if(target == null)
            return;

        transform.position = new Vector3(target.position.x, target.position.y,transform.position.z);
    }
}
