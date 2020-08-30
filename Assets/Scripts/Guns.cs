using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    public static Guns instance;
    public Animator animator;
    public enum GunType 
    {
        Pistol,
        Rifle,
        Knife,
        Hand
    };
    public GunType type = GunType.Pistol;
    public SpriteRenderer rend;

    public float FireRate = 15f;
    public Transform firePoint;
    public GameObject BulletPrefabs;
    public GameObject WeaponObject;
    [Header("총알 갯수")]
    private int currentAmmo;
    public int CenterAmmo =8;
    public int MaxAmmo = 15;
    [Header("재장전 시간")]
    public float ReloadTime = 1f;
    bool isReloading = false;

    public float BulletForce = 20f;
    public GameObject OutAmmo;
    public Transform OutAmmoPos;
    [Header("팔 행동 설정")]
    public GameObject HandObject;
    public enum HandControl
    {
        Idle,
        Pick,
        Throw
    }
    public HandControl handControl;

    [Header("칼이 공격하는가?")]
    public bool isKill = false;

    private float nextTimeToFire = 0f;
    [Header("카메라 흔들림 설정")]
    public float ShakeTheGun = 0.5f;
    [Header("사운드 설정")]
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip ThrowClip;

    void Awake() {
        instance = this;
    }
    void Start()
    {
        currentAmmo = MaxAmmo;
    }
    void Update()
    {
        switch(type)
        {
            case GunType.Pistol : 
                Pistol_Controller();
                break;
            case GunType.Rifle :
                Rifle_Controller();
                break;
            case GunType.Knife :
                Knife_Controller();
                break;
        }
    }


    void FixedUpdate()
    {
        switch(type)
        {
            case GunType.Hand :
                Hand_Controller();
                break;
        }
    }
    void HandPick()
    {
        // 만약 무기 오브젝트가 있으면, 기존 무기는 갖다버립니다.
        if(WeaponArms.instance.isPick == true) {
            Destroy(Guns.instance.gameObject);
        }

        GameObject HandClone = Instantiate(HandObject); // 무기 장착

        WeaponArms.instance.Guns = HandClone.gameObject; // WeaponArm 스크립트의 Guns 컴포넌트에 적용시킴.
        HandClone.transform.parent = PlayerController.instance.GunArm; // 부모 장착
        HandClone.transform.position = PlayerController.instance.GunArm.position; // 총기 트랜스폼 포지션으로 설정
        HandClone.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // 회전 설정을 0f으로 초기화
        HandClone.transform.localScale = new Vector3(1f, 1f, 1f); // 스케일을 1f로 통일

        Destroy(gameObject); // 오브젝트 없애기
    }

    void GunFire()
    {
        SfxManage.instance.SfxPlay(audioSource, audioClip);
        GameObject bullet = Instantiate(BulletPrefabs, firePoint.position, firePoint.rotation);

        Instantiate(OutAmmo, OutAmmoPos.position, OutAmmoPos.rotation);

        Rigidbody2D the2D = bullet.GetComponent<Rigidbody2D>();
        the2D.AddForce(firePoint.right * BulletForce, ForceMode2D.Impulse);
        currentAmmo--;
    }

    private void Pistol_Controller()
    {
        if(isReloading)
            return;

        if(currentAmmo <= 0) {
            StartCoroutine(Reload());
        }

        if(currentAmmo <= CenterAmmo) {
            Debug.Log("총이 절반 남음.");
        }

        if(currentAmmo <= 3) {
            Debug.LogWarning("총이 얼마 안 남음.");
        }

        if(Input.GetButtonDown("Fire1")) 
        {
            CameraShake.instance.ShakeCamera(1f, ShakeTheGun);
            
            animator.SetTrigger("CanFire");
        }

        if(Input.GetButtonDown("Fire2"))
        {
            HandPick();
            DropTheObject();
        }
        
        Vector2 GunMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        animator.SetFloat("Hor", GunMouse.x); // 좌우 움직임 애니메이션
        animator.SetFloat("Ver", GunMouse.y); // 위아래 움직임 애니메이션

        LayerOfWeapon();
    }

    private void Rifle_Controller()
    {
        if(isReloading)
            return;

        if(currentAmmo <= 0) {
            StartCoroutine(Reload());
        }
        
        if(currentAmmo <= CenterAmmo) {
            Debug.Log("총이 절반 남음.");
        }

        if(currentAmmo <= 3) {
            Debug.LogWarning("총이 얼마 안 남음.");
        }

        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire) 
        {
            nextTimeToFire = Time.time + 1f / FireRate;
            CameraShake.instance.ShakeCamera(1.2f, ShakeTheGun);

            animator.SetTrigger("CanFire");            
        }

        if(Input.GetButtonDown("Fire2"))
        {
            HandPick();
            DropTheObject();
        }
        
        Vector2 GunMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        animator.SetFloat("Hor", GunMouse.x); // 좌우 움직임 애니메이션
        animator.SetFloat("Ver", GunMouse.y); // 위아래 움직임 애니메이션

        LayerOfWeapon();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("Reload", true);
        yield return new WaitForSeconds(ReloadTime - .25f);
        animator.SetBool("Reload", false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = MaxAmmo;
        isReloading = false;
    }
    private void Hand_Controller()
    {
        Pickup();
        LayerOfWeapon();
    }

    private void Knife_Controller()
    { 
        if(Input.GetButton("Fire1")) 
        {
            if(Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0) 
            {
               isKill = true;
            } 
            else 
            {
                isKill = false;
            }
        }

        if(Input.GetButtonDown("Fire2"))
        {   
            HandPick(); 

            GameObject bullet = Instantiate(BulletPrefabs, firePoint.position, firePoint.rotation);
            Rigidbody2D the2D = bullet.GetComponent<Rigidbody2D>();
            the2D.AddForce(firePoint.right * BulletForce, ForceMode2D.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            HandPick();
            DropTheObject();
        }
    
        LayerOfWeapon();
    }

    void DropTheObject()
    {
        SfxManage.instance.SfxPlay(audioSource, ThrowClip);    
            
        GameObject Objects = Instantiate(WeaponObject, firePoint.position, firePoint.rotation);
        Rigidbody2D the2D = Objects.GetComponent<Rigidbody2D>();
        ObjectController obj = Objects.GetComponent<ObjectController>();
        the2D.AddForce(firePoint.right * obj.throwSpeed, ForceMode2D.Impulse);

        Vector3 dir;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        dir.x = mousePos.x - this.transform.position.x;
        dir.y = mousePos.y - this.transform.position.y;
        dir.z = 0f;
            
        obj.setDirection(dir);

        Destroy(this.gameObject);
    }

    void Pickup()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector3.up, 1f);

        if(hit) 
        {
            
            if(hit.collider.tag == "Object")
            {
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space) && handControl == HandControl.Idle) 
                {
                    handControl = HandControl.Pick;
                    SfxManage.instance.SfxPlay(audioSource, audioClip);   
                } 
                else if(handControl == HandControl.Pick)
                {
                    ObjectController obj = hit.collider.gameObject.GetComponent<ObjectController>();                        

                    if(Input.GetButton("Fire1") || Input.GetButtonDown("Fire1"))
                    {
                        SfxManage.instance.SfxPlay(audioSource, ThrowClip);                        
                        PlayerController.instance.BasicSpeed = PlayerController.instance.SettingSpeed;

                        Rigidbody2D the2D = hit.collider.gameObject.GetComponent<Rigidbody2D>();

                        the2D.AddForce(firePoint.right * obj.throwSpeed, ForceMode2D.Impulse);
                        obj.isPick = false;  
                        obj.isThrow = true;           

                        handControl = HandControl.Idle;       
                    }      

                    if(Input.GetButton("Fire2") || Input.GetButtonDown("Fire2"))
                    {
                        PlayerController.instance.BasicSpeed = PlayerController.instance.SettingSpeed;
                        obj.isPick = false;            

                        handControl = HandControl.Idle;       
                    }   
                }

                if(handControl == HandControl.Pick)
                {
                    PlayerController.instance.BasicSpeed = hit.collider.gameObject.GetComponent<ObjectController>().MoveSpeed;
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().MovePosition(firePoint.position + firePoint.right * Time.fixedDeltaTime);

                    ObjectController obj = hit.collider.gameObject.GetComponent<ObjectController>();
                    obj.isPick = true; 
                }
            }

            if(hit.collider.tag == "Object_Faint")
            {
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space) && handControl == HandControl.Idle) 
                {
                    handControl = HandControl.Pick;
                    SfxManage.instance.SfxPlay(audioSource, audioClip);  
                } 
                else if(handControl == HandControl.Pick)
                {
                    ObjectController obj = hit.collider.gameObject.GetComponent<ObjectController>();                    
                    obj.isPick = false; 

                    if(Input.GetButton("Fire1") || Input.GetButtonDown("Fire1"))
                    {
                        SfxManage.instance.SfxPlay(audioSource, ThrowClip);                        
                        PlayerController.instance.BasicSpeed = PlayerController.instance.SettingSpeed;

                        Rigidbody2D the2D = hit.collider.gameObject.GetComponent<Rigidbody2D>();

                        the2D.AddForce(firePoint.right * obj.throwSpeed, ForceMode2D.Impulse);
                        obj.isPick = false;  
                        obj.isThrow = true;           

                        handControl = HandControl.Idle;       
                    }  

                    if(Input.GetButton("Fire2") || Input.GetButtonDown("Fire2"))
                    {
                        PlayerController.instance.BasicSpeed = PlayerController.instance.SettingSpeed;
                        obj.isPick = false;            

                        handControl = HandControl.Idle;       
                    }     
                }

                if(handControl == HandControl.Pick)
                {
                    PlayerController.instance.BasicSpeed = hit.collider.gameObject.GetComponent<ObjectController>().MoveSpeed;
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().MovePosition(firePoint.position + firePoint.right * Time.fixedDeltaTime);

                    ObjectController obj = hit.collider.gameObject.GetComponent<ObjectController>();
                    obj.isPick = true; 
                }
            }
        } 
    }

    void LayerOfWeapon()
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
}
