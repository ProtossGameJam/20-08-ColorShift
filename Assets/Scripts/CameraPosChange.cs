using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosChange : MonoBehaviour
{
    /* 이 스크립트는 맵의 크기를 고려하고, 사용하시는것을 권장드려요.*/

    public Transform ChangePoint; // 구역 위치를 카메라 타겟으로 설정합니다
    public float CameraSeeSize = 5f; // 카메라 화면 크기를 조정합니다.
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            CameraController.instance.target = ChangePoint;
            Camera.main.orthographicSize = CameraSeeSize;
        }
    }
}
