using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damaged = 1;  // 공격력
    public float lifeTime = 3f; // 유지시간

    public GameObject bulletHolePrefab;

    private Rigidbody rb;

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime); // 3초 뒤 자동 파괴
    }

    void FixedUpdate()
    {
        // 현재 속도를 기반으로 이번 프레임에 이동할 거리를 계산
        float distanceThisFrame = rb.linearVelocity.magnitude * Time.fixedDeltaTime;

        // 이동할 거리만큼 앞에서 레이캐스트를 쏴서 미리 충돌을 감지
        // transform.forward는 총알이 날아가는 방향을 의미합니다.
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distanceThisFrame))
        {
            // 총알 자국 생성 함수 호출
            CreateBulletHole(hit);

            Destroy(gameObject);
        }
    }

    void CreateBulletHole(RaycastHit hit)
    {
        // 총알 자국 프리팹이 설정되어 있고, 부딪힌 대상이 'Wall' 태그를 가졌을 때
        if (bulletHolePrefab != null && hit.collider.CompareTag("Wall"))
        {
            
            // 회전 계산: 데칼이 벽의 표면과 같은 방향을 바라보도록 설정 (hit.normal은 표면이 바라보는 방향)
            Quaternion decalRotation = Quaternion.LookRotation(hit.normal);

            // 위치 계산: 부딪힌 지점(hit.point)에서 표면 방향으로 아주 살짝(0.01m) 띄워서 생성 (겹침 방지)
            Vector3 decalPosition = hit.point + hit.normal * 0.01f;


            // 데칼 생성
            GameObject bulletHole = Instantiate(bulletHolePrefab, decalPosition, decalRotation);

            // 생성된 데칼을 부딪힌 벽의 자식으로 만들어, 벽이 움직일 때 같이 움직이게 함
            bulletHole.transform.SetParent(hit.collider.transform);

            // 생성된 총알 자국이 10초 뒤에 자동으로 사라지게 만듦
            Destroy(bulletHole, 10f);
        }
    }


}