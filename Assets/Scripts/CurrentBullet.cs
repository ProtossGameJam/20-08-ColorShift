using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentBullet : MonoBehaviour
{
    public float splitBulletTime;
    public GameObject bullet;
    public Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        splitBulletTime -= Time.deltaTime;

        if(splitBulletTime <= 0)
        {
            BulletHell();
            Destroy(gameObject);
        }
    }

    public void BulletHell()
    {
        for (int i = 0; i < 360; i += Random.Range(50,100))
        {
            GameObject temp = Instantiate(bullet);
            
            temp.transform.position = target.position;

            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
