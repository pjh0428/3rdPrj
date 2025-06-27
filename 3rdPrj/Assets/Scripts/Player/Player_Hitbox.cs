using UnityEngine;

public class Player_Hitbox : MonoBehaviour
{
    public Player player; // 플레이어 스크립트: hp관리


    void Start()
    {
        if (player == null)
            player = GetComponentInParent<Player>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적과 충돌 시 플레이어의 hp를 감소시킴
            player.TakeDamage(20);

        }
    }
}
