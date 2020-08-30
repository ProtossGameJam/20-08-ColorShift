using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets : MonoBehaviour
{
    public enum Bullets
    {
        Updates,
        Switch
    };

    public enum Types
    {
        EnemySwitchAttack,
        BasicAttack
    };
    public Bullets bullet;
    public Types AttackType = Types.BasicAttack;

    public string Target = "Player";
    public string ColorSwitch;
    public Color SwitchColor;

    public GameObject MuzzleFlash;
    [Header("변경될 오브젝트 레이어 마스크 목록")]
    public string LayerMask_Name;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(MuzzleFlash, transform.position, transform.rotation);     

        if(collision.gameObject.tag == Target)
        {
            if(AttackType == Types.EnemySwitchAttack)
            {
                if(collision.gameObject.GetComponent<EnemyController>().ColorMan != ColorSwitch)
                {
                    Destroy(collision.gameObject);
                    CameraShake.instance.ShakeCamera(.8f, .08f);
                    PlayerController.instance.GetComponent<SpriteRenderer>().color = SwitchColor;
                    PlayerController.instance.gameObject.name = collision.gameObject.name;
                    PlayerController.instance.gameObject.tag = collision.gameObject.tag;
                }
            }
        }

        
        if(collision.gameObject.name == "UpdateObject")
        {             
            Destroy(gameObject);

            if(bullet == Bullets.Updates)
            {
                collision.gameObject.GetComponent<GroundController>().isGroundUpdate = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Wall") 
        {
            Instantiate(MuzzleFlash, transform.position, transform.rotation); 
            Destroy(gameObject);    
        }
            
        if(other.gameObject.tag == "Object")
        {
            Instantiate(MuzzleFlash, transform.position, transform.rotation);

            if(bullet == Bullets.Switch)
            {
                if(other.gameObject.GetComponent<ObjectController>().ColorNation != ColorSwitch)
                {
                    other.gameObject.GetComponent<ObjectController>().ColorNation = ColorSwitch;
                    other.gameObject.GetComponent<ObjectController>().gameObject.layer = LayerMask.NameToLayer(LayerMask_Name);                    
                    other.gameObject.GetComponent<SpriteRenderer>().sprite = other.gameObject.GetComponent<ObjectController>().SwitchSprite;
                }
            }
        }

        if(other.gameObject.tag == "Object_Faint")
        {
            Instantiate(MuzzleFlash, transform.position, transform.rotation);

            if(bullet == Bullets.Switch)
            {
                if(other.gameObject.GetComponent<ObjectController>().ColorNation != ColorSwitch)
                {
                    other.gameObject.GetComponent<ObjectController>().ColorNation = ColorSwitch;
                    other.gameObject.GetComponent<ObjectController>().gameObject.layer = LayerMask.NameToLayer(LayerMask_Name);                
                    other.gameObject.GetComponent<SpriteRenderer>().sprite = other.gameObject.GetComponent<ObjectController>().SwitchSprite;
                }
            }
        }
    }
}
