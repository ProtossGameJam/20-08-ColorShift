using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("소속팀")]
    public string ColorMan;
    public string EnemyColor;
    [Header("애니메이션")]
    public Animator animator;
    [Header("움직임")]
    private float MoveSpeed = 5f;
    public float BasicSpeed = 5f;
    [Header("시야 각도")]
    public float CustomAngle = 360f;
    [Header("정찰 위치 설정")]
    public Transform[] MoveSpot;
    int MoveSpotIndex = 0;
    [Header("타겟 설정")]
    public GameObject target;
    [Header("정찰모드?")]
    Vector3 direction;
    [Header("대기시간")]
    private float WaitTime;
    public float StartWaitTime;
    [Header("적 상태 ")]
    public EnemyState enemyState = EnemyState.Idle;
    [Header("적색상태")]
    public Color EnemyColorCode;
    public enum EnemyState
    {
        Idle,
        Fire,
        Faint
    };
    [Header("레이어 설정")]
    public string DefaultTheLayer;
    public string BasicLayer;
    [Header("총설정")]
    public EnemyGunManager enemyGunManager;
    [Header("죽음 사운드")]
    public AudioSource audioSource;
    public AudioClip swordAttackDeath;
    public AudioClip FaintSound;
    public bool isDead = false;
    [Header("보스이냐, 적이냐")]
    public EnemyType types = EnemyType.Enemy;
    [Header("적이 Idle로 돌아오는데 5초 남음.")]
    float BackIdle = 5;
    public enum EnemyType
    {
        Enemy,
        Boss
    };

    

    void Start() 
    {
        WaitTime = StartWaitTime;
    }
    void Update()
    {
        if(!isDead)
        {
            if(enemyState == EnemyState.Faint) 
            {
                animator.SetBool("Dead", true);
                enemyGunManager.isHide = false;            
            }

            if(enemyState == EnemyState.Idle) 
            {
                EnemyPatrol();
                MoveSpeed = BasicSpeed;
                
                enemyGunManager.isHide = false;
                animator.SetFloat("Hor", MoveSpot[MoveSpotIndex].position.x - transform.position.x);
                animator.SetFloat("Ver", MoveSpot[MoveSpotIndex].position.y - transform.position.y);

                if(target != null)
                {
                    AnimationTheEnemy();
                }

                if(target == null) 
                {
                    MoveSpeed = BasicSpeed;
                    animator.SetFloat("Hor", MoveSpot[MoveSpotIndex].position.x - transform.position.x);
                    animator.SetFloat("Ver", MoveSpot[MoveSpotIndex].position.y - transform.position.y);
                }
            } 

            if(enemyState == EnemyState.Fire)
            {
                BackIdle -= Time.deltaTime;

                if(BackIdle <= 0) {
                    enemyState = EnemyState.Idle;
                    BackIdle = 5;
                }

                MoveSpeed = 0f;
                animator.SetFloat("Hor", (direction.x - transform.position.x));
                animator.SetFloat("Ver", (direction.y - transform.position.y));
                enemyGunManager.isHide = true;
            }
        }
    }

    private void EnemyPatrol()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, MoveSpot[MoveSpotIndex].transform.position, MoveSpeed * Time.deltaTime);

        if(transform.position == MoveSpot[MoveSpotIndex].transform.position) 
        {
            if(WaitTime <= 0)
            {
                MoveSpotIndex += 1;
                WaitTime = StartWaitTime;
            } 
            else 
            {
                WaitTime -= Time.deltaTime;
            }
        }

        if(MoveSpotIndex == MoveSpot.Length) {
            MoveSpotIndex = 0;
        }
    }

    private void AnimationTheEnemy()
    {
        Vector2 targetDir = target.transform.position - transform.position;
        float targetDirAngle = GetLookAngle(targetDir);
        float angle = GetLookAngle(MoveSpot[MoveSpotIndex].position - transform.position);
        

        if (targetDirAngle > angle - (CustomAngle / 2) && angle + (CustomAngle / 2) > targetDirAngle)  
        {
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Pass") | (1 << LayerMask.NameToLayer("object")) | (1 << LayerMask.NameToLayer("enemybullet")));
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, targetDir, Mathf.Infinity, layerMask);
            
            if (hit2D)
            {
                direction = hit2D.collider.transform.position;
                enemyState = EnemyState.Idle;

                if (hit2D.collider.tag == "Player")
                {
                    enemyState = EnemyState.Fire;
                }

                if(hit2D.collider.tag == EnemyColor)
                {
                    enemyState = EnemyState.Fire;                    
                }
            }
        }

        Vector2 offset = MoveSpot[MoveSpotIndex].position - transform.position;
        animator.SetFloat("speed", offset.sqrMagnitude);
    }

    private float GetLookAngle(Vector2 dir)
    {
        float dirX = (dir).normalized.x;
        float rawAngle = Mathf.Acos((float)dirX) * 180.0f / Mathf.PI;

        float realAngle = dirX > 0 ? Mathf.Abs(rawAngle) : -Mathf.Abs(rawAngle);

        return realAngle;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            StartCoroutine(Death());
        }
        
        if(other.gameObject.tag == "PlayerBullet")
        {  
            if(types == EnemyType.Enemy)
            {
                if(other.gameObject.GetComponent<BulletObject>().Color !=  ColorMan)
                {
                    StartCoroutine(Death());

                    PlayerController.instance.GetComponent<SpriteRenderer>().color = EnemyColorCode;
                    PlayerController.instance.gameObject.name = gameObject.name;
                    PlayerController.instance.gameObject.tag = gameObject.tag;
                } 
            }

            if(types == EnemyType.Boss)
            {
                if(other.gameObject.GetComponent<BulletObject>().Color !=  ColorMan)
                {
                    CameraShake.instance.ShakeCamera(.8f, .08f);
                    //BossHealths.instance.TakeDamage(1);

                    if(BossHealths.instance.Health <= 0)
                    {
                        StartCoroutine(Death());
                    }
                }
            }
        }

        if(other.collider.tag == "Sword")
        {
            if(Guns.instance.isKill == true)
            {  
                if(types == EnemyType.Enemy)
                {
                    StartCoroutine(Death());
                    CameraShake.instance.ShakeCamera(.8f, .08f);
                }

                if(types == EnemyType.Boss)
                {
                    BossHealths.instance.TakeDamage(1);
                    CameraShake.instance.ShakeCamera(.8f, .08f);

                    if(BossHealths.instance.Health <= 0)
                    {
                        StartCoroutine(Death());
                    }
                }                          

            }
        }

        if(other.collider.tag == "SwitchSword")
        {
            if(Guns.instance.isKill == true)
            {   
                if(other.gameObject.GetComponent<BulletObject>().Color !=  ColorMan)
                {             
                    StartCoroutine(Death());

                    CameraShake.instance.ShakeCamera(.8f, .08f);
                    PlayerController.instance.GetComponent<SpriteRenderer>().color = EnemyColorCode;
                    PlayerController.instance.gameObject.name = other.gameObject.name;
                    PlayerController.instance.gameObject.tag = other.gameObject.tag;
                }
            }
        }

        if(other.gameObject.tag == "Object")
        {
            if(other.gameObject.GetComponent<ObjectController>().isThrow)
            {
                if(types == EnemyType.Enemy)
                {
                    if(ColorMan != other.gameObject.GetComponent<ObjectController>().ColorNation)
                    {
                        StartCoroutine(Death());
                    }
                }

                if(types == EnemyType.Boss)
                {
                    if(ColorMan != other.gameObject.GetComponent<ObjectController>().ColorNation)
                    {
                        BossHealths.instance.TakeDamage(3);
                        CameraShake.instance.ShakeCamera(.8f, .08f);
                        SfxManage.instance.SfxPlay(audioSource, swordAttackDeath);

                        if(BossHealths.instance.Health <= 0)
                        {
                            StartCoroutine(Death());
                        }
                    }
                }
            }

            if(other.gameObject.GetComponent<ObjectController>().isPick)
            {
                if(types == EnemyType.Enemy)
                {
                    if(ColorMan != other.gameObject.GetComponent<ObjectController>().ColorNation)
                    {
                        StartCoroutine(Death());
                    }
                }

                if(types == EnemyType.Boss)
                {
                    if(ColorMan != other.gameObject.GetComponent<ObjectController>().ColorNation)
                    {
                        CameraShake.instance.ShakeCamera(.8f, .08f);                        
                        BossHealths.instance.TakeDamage(3);
                        SfxManage.instance.SfxPlay(audioSource, swordAttackDeath);
                        
                        if(BossHealths.instance.Health <= 0)
                        {
                            StartCoroutine(Death());
                        }
                    }
                }
            }
        }

        if(other.gameObject.tag == "Object_Faint")
        {
            if(other.gameObject.GetComponent<ObjectController>().isThrow)
            {
                if(types == EnemyType.Enemy)
                {
                    StartCoroutine(Faint());
                }
            }

            if(other.gameObject.GetComponent<ObjectController>().isPick)
            {
                if(types == EnemyType.Enemy)
                {
                    StartCoroutine(Faint());
                }
            }
        }
    }
    public IEnumerator Death()
    {
        SfxManage.instance.SfxPlay(audioSource, swordAttackDeath);
        KillCountSender.instance.count += 1;

        isDead = true;
        enemyGunManager.isHide = false;
        animator.SetBool("TheDead",true);

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = DefaultTheLayer;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder  = 1;

        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        CameraShake.instance.ShakeCamera(.8f, .08f);

        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    IEnumerator Faint()
    {
        SfxManage.instance.SfxPlay(audioSource, FaintSound);

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = DefaultTheLayer;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder  = 0;        
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        CameraShake.instance.ShakeCamera(1.2f, .1f);
        enemyState = EnemyState.Faint;
        yield return new WaitForSeconds(15);
        animator.SetBool("Dead", false);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = BasicLayer;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder  = 1; 
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        enemyState = EnemyState.Idle;
    }
}
