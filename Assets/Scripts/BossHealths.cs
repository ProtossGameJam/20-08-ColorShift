using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealths : MonoBehaviour
{
    public EnemyController enemyController;
    public static BossHealths instance;
    public int Health = 9;

    void Awake() {
        instance = this;
    }

    public void TakeDamage(int TakeDamage)
    {
        Health -= TakeDamage;

        if(Health <= 0)
        {
            Health = 0;
        }
    }
    
}
