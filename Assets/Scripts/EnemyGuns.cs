using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuns : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer rend;
    public Transform FirePoint;
    public GameObject[] Bullet;
    public float BulletForce = 20f;
    public float FireRate = 5f;
    float nextTimeToFire = 0f;
    public string EnemyTarget;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Transform points;
    public GameObject Targets;
    public Vector3 direction;
    public string objectTarget;
    
    void OnEnable()
    {
        animator.ResetTrigger("CanFire");  

        if(Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / FireRate;
            Shoot();
        }        
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        points.rotation = Quaternion.Euler(0, 0, 90f);
    }

    void Update()
    {
        if(Targets == null)
            return;

        LookTheWayPoint_GunArm();
        
        direction = Targets.transform.position - transform.position;  
            
        animator.SetFloat("Hort", direction.x); // 좌우 움직임 애니메이션
        animator.SetFloat("Vert", direction.y); // 위아래 움직임 애니메이션 


    }
    
    void LateUpdate()
    {    
        GunAngles();   
        
        if(Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / FireRate;
            Shoot();
        }
    }

    public void Shoot()
    {
        int layerMask = ((1 << LayerMask.NameToLayer("enemybullet")) | (1 << LayerMask.NameToLayer(objectTarget)));
        layerMask  = ~layerMask ;
        RaycastHit2D hit2D = Physics2D.Raycast(FirePoint.position, FirePoint.right, Mathf.Infinity, layerMask);

        if (hit2D)
        {        
            Targets = null;

            if(hit2D.collider.tag == "Player")
            {
                Targets = hit2D.collider.gameObject;
                
                animator.SetTrigger("CanFire");
                GameObject bullet = Instantiate(Bullet[0], FirePoint.position, FirePoint.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = (hit2D.transform.position - transform.position).normalized * BulletForce;
            }

            if(hit2D.collider.tag == EnemyTarget)
            {              
                Targets = hit2D.collider.gameObject; 

                animator.SetTrigger("CanFire");
                GameObject bullet = Instantiate(Bullet[1], FirePoint.position, FirePoint.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = (hit2D.transform.position - transform.position).normalized * BulletForce;           
            }
        }
    }

    public void ShootSound()
    {
        SfxManage.instance.SfxPlay(audioSource, audioClip);
    }

    void GunAngles()
    {
        if(rend == null)
            return;

        float angle = transform.parent.rotation.eulerAngles.z;

        if (angle < 180) 
        {
            rend.sortingOrder = 0;
            rend.sortingLayerName = "Body";
        } 
        else 
        {
            rend.sortingOrder = 0;
            rend.sortingLayerName = "Gun";
        }
    }

    private void LookTheWayPoint_GunArm()
    {
        Vector3 Rotations = transform.localPosition;

        if(direction.x < Rotations.x)
        {
            points.localScale = new Vector3(1f,-1f,1f);
        }
        else
        {
            points.localScale = Vector3.one;
        }

        Vector2 offset = new Vector2(direction.x - Rotations.x, direction.y - Rotations.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        points.rotation = Quaternion.Euler(0, 0, angle);
    }
}
