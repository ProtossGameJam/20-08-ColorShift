using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGround : MonoBehaviour
{
    public string[] ColorSelect;
    private void OnTriggerEnter2D (Collider2D other)
    {
        if(other.gameObject.name == ColorSelect[0])
        {
            Destroy(other.gameObject);
        }
    }
}
