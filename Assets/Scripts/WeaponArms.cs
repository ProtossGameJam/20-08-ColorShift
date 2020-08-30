using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArms : MonoBehaviour
{
    public static WeaponArms instance;

    public bool isPick;
    public GameObject Guns; // 무기 장착 컴포넌트

    void Awake() {
        instance = this;
    }

    void Start() {
        Guns = GameObject.FindGameObjectWithTag("Weapon"); // 만약 Weapon 태그를 가진 오브젝트가 있다면 컴포넌트에 적용시킵니다.
    }
    public void Update() {
        if(Guns) // 총 컴포넌트에 오브젝트가 있으면 isPick이 참이 됩니다. 그렇지 않을경우 거짓이 됩니다. 
        {
            isPick = true;
        } 
        else // 총 컴포넌트에 오브젝트가 없으면 isPick이 거짓이 됩니다. 그렇지 않을 경우 참이 됩니다. 
        {
            isPick = false;
            Guns = null; // 무기 컴포넌트에 none으로 설정해줌.
        }
    }
}
