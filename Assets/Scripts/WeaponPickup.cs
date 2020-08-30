using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Guns WeaponObject;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        // 만약 무기 오브젝트가 있으면, 기존 무기는 갖다버립니다.
        if(WeaponArms.instance.isPick == true) {
            Destroy(Guns.instance.gameObject);
        }
        
        Guns gunClone = Instantiate(WeaponObject); // 무기 장착

        WeaponArms.instance.Guns = gunClone.gameObject; // WeaponArm 스크립트의 Guns 컴포넌트에 적용시킴.
        gunClone.transform.parent = PlayerController.instance.GunArm; // 부모 장착
        gunClone.transform.position = PlayerController.instance.GunArm.position; // 총기 트랜스폼 포지션으로 설정
        gunClone.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // 회전 설정을 0f으로 초기화
        gunClone.transform.localScale = new Vector3(1f, 1f, 1f); // 스케일을 1f로 통일

        Destroy(gameObject); // 오브젝트 없애기
    }
}
