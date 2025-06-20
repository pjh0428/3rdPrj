using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Rifle : MonoBehaviour
{
    [Header("총기정보")]
    //[SerializeField] private float Damage = 10f; // 총알 피해량
    [SerializeField] private float FireRate = 0.2f; // 발사 간격
    [SerializeField] int MagazineSize = 30; // 탄창 크기
    [SerializeField] private float ReloadTime = 2.0f; // 재장전 시간

    [Header("발사관련정보")]
    public Transform FirePoint; // 총구
    public GameObject FireFlash; // 총구 섬광
    public AudioClip ShootSound; // 발사 사운드
    public AudioClip ReloadSound; // 재장전 사운드

    public int CurrentAmmo; // 현재 탄약
    private bool isReloading = false; // 재장전 여부
    private float NextFireTime = 0f; // 다음 발사 가능 시간
    private AudioSource AudioSource; 




    void Start()
    {
        CurrentAmmo = MagazineSize; // 현재 탄약 초기화
        AudioSource = gameObject.AddComponent<AudioSource>();
    }

   
    void Update()
    {
        // 재장전 중이 아닐 때만 발사
        if (isReloading)
            return;

        // 발사함수 - 마우스 좌클릭
        if (Input.GetMouseButton(0) && Time.time >= NextFireTime)
        {
            NextFireTime = Time.time + FireRate; // 시간갱신
            Fire();
            Debug.Log("발사, 현재 탄약: " + CurrentAmmo);
        }

        // 재장전(R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 재장전 코루틴
            StartCoroutine(Reload());
        }

        void Fire()
        {
            if (CurrentAmmo <= 0)
            {
                // 총알이 없으면 재장전 시도
                StartCoroutine(Reload());
                return;
            }

            CurrentAmmo--;

            // 총구 섬광 이펙트 생성
            if (FireFlash != null)
                Instantiate(FireFlash, FirePoint.position, FirePoint.rotation);

            // 발사 사운드 재생
            if (ShootSound != null)
                AudioSource.PlayOneShot(ShootSound);

            // 발사 로직 추가필요
        }

        IEnumerator Reload()
        {
            // 재장전 중이거나, 탄창이 가득 차 있으면 재장전하지 않음
            if (isReloading || CurrentAmmo == MagazineSize)
            {
                yield break; // 코루틴 즉시 종료
            }

            isReloading = true;
            Debug.Log("재장전 시작");

            // 재장전 사운드 재생
            if (ReloadSound != null)
                AudioSource.PlayOneShot(ReloadSound);

            // 재장전 애니메이션 실행함수 필요

            // 재장전 시간만큼 대기
            yield return new WaitForSeconds(ReloadTime);

            // 총알 채우기
            CurrentAmmo = MagazineSize;
            isReloading = false;
            Debug.Log("재장전 완료");
        }



    }
}
