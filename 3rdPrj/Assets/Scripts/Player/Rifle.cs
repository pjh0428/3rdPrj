using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

public class Rifle : MonoBehaviour
{
    [Header("총기정보")]
    [SerializeField] private float FireRate = 0.1f; // 발사속도
    [SerializeField] int MagazineSize = 1000; // 장탄수
    [SerializeField] private float ReloadTime = 2.0f; // 재장전 시간

    [Header("발사관련정보")]
    public Transform FirePoint; // 총구
    public GameObject FireFlash; // 총기화염

    [Header("총알관련정보")]
    public GameObject BulletPrefab; // 총알프리팹
    public float BulletSpeed = 50f; // 총알속도

    [Header("사운드관련")]
    public AudioClip ShootSound; // 발사소리
    public AudioClip ReloadSound; // 재장전소리


    [SerializeField] private int CurrentAmmo; // 현재 장탄수
    private bool isReloading = false; // 재장전 여부
    private float NextFireTime = 0.1f; // 다음 발사 시간
    private AudioSource AudioSource;

    private AimController _aimController;
    private CinemachineImpulseSource _impulseSource;


    void Start()
    {
        CurrentAmmo = MagazineSize; // 장탄수 초기화
        AudioSource = gameObject.AddComponent<AudioSource>();
        _aimController = GetComponentInParent<AimController>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }


    void Update()
    {
        // 재장전 중 아니면 발사 가능
        if (isReloading)
            return;

        // 마우스 좌클릭 발사
        if (Input.GetMouseButton(0) && Time.time >= NextFireTime)
        {
            NextFireTime = Time.time + FireRate;
            Fire();
            Debug.Log("잔여 장탄수: " + CurrentAmmo);
        }

        // 재장전 = R
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        if (CurrentAmmo <= 0)
        {
            // 장탄이 0이면 자동 재장전
            StartCoroutine(Reload());
            return;
        }

        CurrentAmmo--;

        // 총기화염 표시
        if (FireFlash != null)
        {
            GameObject flash =Instantiate(FireFlash, FirePoint.position, FirePoint.rotation);
            Destroy(flash, 0.1f);
        }

        // 발사소리 출력
        if (ShootSound != null)
            AudioSource.PlayOneShot(ShootSound);

        if (_impulseSource != null)
        {
            _impulseSource.GenerateImpulse();
        }

        if (BulletPrefab != null)
        {         
            Vector3 direction = (_aimController.AimPoint - FirePoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = direction * BulletSpeed;
            }
        }
    }

    IEnumerator Reload()
    {
        // 장탄이 다 차있으면 재장전x
        if (isReloading || CurrentAmmo == MagazineSize)
        {
            yield break;
        }

        isReloading = true;
        Debug.Log("재장전 중");

        if (ReloadSound != null)
            AudioSource.PlayOneShot(ReloadSound);

        yield return new WaitForSeconds(ReloadTime);

        CurrentAmmo = MagazineSize;
        isReloading = false;
        Debug.Log("재장전 끝");
    }

}

    

