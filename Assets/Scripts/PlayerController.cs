using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("애니메이션")]
    public Animator animator;
    [Header("움직임 속도")]
    private float moveSpeed = 5f; // 현재 속도
    public float BasicSpeed = 5f; // 설정한 속도
    public float SettingSpeed = 5f;
    [Header("리지드바디2D")]
    public Rigidbody2D rigidbodys;
    [Header("움직임 벡터2")]
    public Vector2 movement; // 움직임 위치
    private Vector2 mousePos; // 마우스 위치
    [Header("크로스헤어")]
    public Transform CrossHair; // 크로스헤어
    [Header("총 장착 부분")]
    public Transform GunArm;
    [Header("제한시간 설정")]
    private float Timer = 10f;
    public float SelectTimer = 10f;
    [Header("적이 파괴되는 소리")]
    public AudioSource audioSource;
    public AudioClip DeathSound;
    public bool isDead = false;

    void Awake() {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Timer = SelectTimer;
        BasicSpeed = SettingSpeed;
    }
    void Update()
    {
        CrossHair_Controller();
        GunArms();

        SpyController();
    }

    void FixedUpdate()
    {
        if(!isDead)
        {
            Character_Movement();
            SpeedSetting();
        }
    }

    private void Character_Movement()
    {
        // 이 부분은 컨트롤러를 설정하게 만들었습니다..
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveSpeed = Mathf.Clamp(movement.magnitude, 0.0f,1.0f); // 미끄러움 방지 
        movement.Normalize(); // movement가 1의 magnitude를 갖도록 설정합니다. (출처 : 유니티3D 공식 Api)

        // 리지드바디를 이용해, 부드러운 움직임을 구현.
        rigidbodys.velocity = movement * moveSpeed * BasicSpeed;

        // 마우스를 이용해 캐릭터의 시야를 변환시켜줍니다.
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        animator.SetFloat("Hor", movement.x + mousePos.x); // 좌우 움직임 애니메이션
        animator.SetFloat("Ver", movement.y + mousePos.y); // 위아래 움직임 애니메이션

        animator.SetFloat("speed", moveSpeed); // 움직임 애니메이션
    }

    private void CrossHair_Controller()
    {
        // 크로스헤어 위치 움직임 설정
        CrossHair.localPosition = new Vector2 (Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2));

        float CursorPosX = CrossHair.localPosition.x; // 크로스헤어 x축 로컬포지션
        float CursorPosY = CrossHair.localPosition.y; // 크로스헤어 y축 로컬포지션


        // 화면 영역 제한
        CursorPosX = Mathf.Clamp(CursorPosX, (-Screen.width / 2 + 50), (Screen.width / 2 - 50));
        CursorPosY = Mathf.Clamp(CursorPosY, (-Screen.height / 2 + 50), (Screen.height / 2 - 50));

        CrossHair.localPosition = new Vector2(CursorPosX, CursorPosY);
    }

    private void GunArms()
    {
        Vector3 screenPoints = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector3 TheMouse = Input.mousePosition;

        if(TheMouse.x < screenPoints.x) {
            GunArm.localScale = new Vector3(1f,-1f,1f);
        } else {
            GunArm.localScale = Vector3.one;
        }
        
        Vector2 offset = new Vector2(TheMouse.x - screenPoints.x, TheMouse.y - screenPoints.y);
        float angles = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        GunArm.rotation = Quaternion.Euler(0,0,angles);
    }

    public void SpyController()
    {
        if(gameObject.name != "Player" || gameObject.tag != "Player")
        {
            Timer -= Time.deltaTime;

            if(Timer <= 0) 
            {
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.color = new Color(1,1,1,1);

                gameObject.name = "Player";
                gameObject.tag = "Player";
                
                Timer = SelectTimer;
            }
        }
    }

    public void SpeedSetting()
    {
        if(BasicSpeed != SettingSpeed)
        {
            BasicSpeed = SettingSpeed;
        }
    }

    public IEnumerator PlayerDeath()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        GunArm.gameObject.SetActive(false);

        Destroy(gameObject.GetComponent<Rigidbody2D>());

        CameraShake.instance.ShakeCamera(.8f, .08f);

        SfxManage.instance.SfxPlay(audioSource, DeathSound);

        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameOverInputHandler>(true).gameObject.SetActive(true);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Object")
        {
            BasicSpeed = collision.gameObject.GetComponent<ObjectController>().MoveSpeed;
        }

        if(collision.gameObject.tag == "EnemyBullet")
        {
            StartCoroutine(PlayerDeath()); 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "Object")
       {
           BasicSpeed = collision.gameObject.GetComponent<ObjectController>().MoveSpeed;
       }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Object")
        {
            BasicSpeed = SettingSpeed;
        }
    }
}
