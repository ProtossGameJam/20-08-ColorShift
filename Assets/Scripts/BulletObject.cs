using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public GameObject MuzzleFlash;
    public string Color = "Blue";
    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(MuzzleFlash, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(MuzzleFlash, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
