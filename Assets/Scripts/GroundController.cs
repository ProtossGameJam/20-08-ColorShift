using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    
    public enum Grounds
    {
        UpdateGround,
        ChangeGround,
        HideGround,
    };

    [Header("GroundType Select")]
    public Grounds Types;

    [Header("UpdateGround")]
    public Vector2 BasicGround,BasicGround_Pos, UpdateGround,UpdateGround_Pos;
    public bool isGroundUpdate = false;
    [Header("ChangeGround")]
    public GameObject TargetGround,SpawnGround;
    public bool isChangeGround = false;
    private float CurrentTimer = 5f;
    public float Timer = 5f;
    [Header("HideGround")]
    public bool isHideGround = false;
    void Start()
    {
        CurrentTimer = Timer;
    }

    void Update()
    {
        switch(Types)
        {
            case Grounds.UpdateGround:
                Update_Ground();
                break;
            case Grounds.ChangeGround:
                Change_Ground();
                break;
            case Grounds.HideGround:
                BeHide_Ground();
                break;

        }
    }

    public void Update_Ground()
    {
        if(isGroundUpdate) 
        {
            transform.localScale = UpdateGround;
            transform.position = UpdateGround_Pos;
            CurrentTimer -= Time.deltaTime;
        }

        if(CurrentTimer <= 0)
        {
            transform.localScale = BasicGround;
            transform.position = BasicGround_Pos;
            isGroundUpdate = false;
            CurrentTimer = Timer;
        }
    }

    public void BeHide_Ground()
    {
        if(isHideGround) {
            SpawnGround.SetActive(true);
        } else {
            SpawnGround.SetActive(false);
        }
    }

    public void Change_Ground()
    {
        if(isChangeGround) {
            TargetGround.SetActive(false);
            SpawnGround.SetActive(true);
        } else {
            TargetGround.SetActive(true);
            SpawnGround.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            if(Types == Grounds.ChangeGround) 
            {
                isChangeGround = true;
            }

            if(Types == Grounds.HideGround)
            {
                isHideGround = true;
            }
        }

        if(other.gameObject.tag == "Object")
        {
            if(Types == Grounds.ChangeGround) 
            {
                isChangeGround = true;
            }

            if(Types == Grounds.HideGround)
            {
                isHideGround = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            if(Types == Grounds.ChangeGround) 
            {
                isChangeGround = false;
            }
        }

        if(other.gameObject.tag == "Object")
        {
            if(Types == Grounds.ChangeGround) 
            {
                isChangeGround = false;
            }

            if(Types == Grounds.HideGround)
            {
                isHideGround = false;
            }
        }
    }
}
