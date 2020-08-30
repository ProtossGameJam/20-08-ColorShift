using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunManager : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject TheGuns;
    public string EnemyTarget;
    public GameObject BasicTarget;
    Vector3 direction;  
    public bool isHide = false;
    public string objectTarget;

    void Start()  
    {  
        TheGuns.GetComponent<EnemyGuns>().Targets = null;
        BasicTarget = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(isHide)
        {
            TheGuns.SetActive(true);            
        }
        else
        {
            TheGuns.SetActive(false);
        }
    }
    void LateUpdate()
    {
        if(BasicTarget != null)
        {
            Vector3 Rotations = transform.localPosition;
            direction = BasicTarget.transform.position;

            Vector2 offset = new Vector2(direction.x - Rotations.x, direction.y - Rotations.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            FirePoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        if(BasicTarget == null) {
            BasicTarget = null;
            return;
        }



        int layerMask = ((1 << LayerMask.NameToLayer("Pass")) | (1 << LayerMask.NameToLayer("enemybullet")) | (1 << LayerMask.NameToLayer(objectTarget)));
        layerMask  = ~layerMask ;
        RaycastHit2D hit2D = Physics2D.Raycast(FirePoint.position, FirePoint.right, 50f, layerMask);

        if (hit2D)
        {        
            TheGuns.GetComponent<EnemyGuns>().Targets = null;

            if(hit2D.collider.tag == "Player")
            {
                TheGuns.GetComponent<EnemyGuns>().Targets = hit2D.collider.gameObject;
            }

            if(hit2D.collider.tag == EnemyTarget)
            {                                                        
                TheGuns.GetComponent<EnemyGuns>().Targets = hit2D.collider.gameObject;
            }
        }
    }
}
