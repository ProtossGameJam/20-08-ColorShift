using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public enum ObjectType
    {
        GunObject,
        BasicObject,
        JustObject,
        BulletObject
    };
    public string ColorNation;
    public Sprite SwitchSprite;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D theRB;
    public ObjectType objectType = ObjectType.BasicObject;
    [Header("회전 속도, 오브젝트 잡으면서 움직이는 속도, 던지는 속도")]
    public float speed = 2f;
    public float MoveSpeed = 3f;
    public float throwSpeed = 10f;
    [Header("오브젝트 던짐에 따라 적이 죽는가?")]
    public bool isThrow = false;
    public bool isPick = false;
    public float ThrowStopSpeed = 2.5f;
    Vector3 direction;
    [Header("총알 사라짐 갯수")]
    public float Timer = 3f;

    void Start() 
    {
        theRB.AddForce(direction * throwSpeed * Time.deltaTime);
        theRB = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        if(objectType == ObjectType.GunObject)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation, new Quaternion(this.transform.rotation.x, this.transform.rotation.y,this.transform.rotation.z -1, this.transform.rotation.w), Time.deltaTime * speed);
        }

        if(objectType == ObjectType.BulletObject) 
        {
            LayerOfObject();
            Timer -= Time.deltaTime;

            if(Timer <= 0) {
                Destroy(gameObject);
            }
        }

        if(theRB.velocity.x < ThrowStopSpeed && theRB.velocity.y < ThrowStopSpeed)
            isThrow = false;
    }

    public void setDirection(Vector3 dir)
    {
        direction = dir;
    }
    void LayerOfObject()
    {
        if(spriteRenderer == null)
            return;

        float angle = transform.rotation.eulerAngles.z;

        if (angle < 180) 
        {
            spriteRenderer.sortingOrder = 0;
            spriteRenderer.sortingLayerName = "Body";
        } 
        else 
        {
            spriteRenderer.sortingOrder = 0;
            spriteRenderer.sortingLayerName = "Gun";
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(objectType == ObjectType.JustObject)
        {
            if(collision.gameObject.name == "CanDestroyObject")
            {
                if(isThrow)
                {             
                    if(collision.gameObject.GetComponent<ObjectController>().ColorNation == ColorNation)
                    {   
                        Destroy(collision.gameObject);
                    }
                }
            }

            if(collision.gameObject.tag == "Switch")
            {
                collision.gameObject.GetComponent<GroundController>().isChangeGround = true;
            }
        }
    
        if(collision.collider.tag == "Sword")
        {
            if(Guns.instance.isKill == true)
            {
                Destroy(gameObject);
            }
        }
    }
}
