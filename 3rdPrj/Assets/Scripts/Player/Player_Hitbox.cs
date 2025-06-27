using UnityEngine;

public class Player_Hitbox : MonoBehaviour
{
    public Player player; // �÷��̾� ��ũ��Ʈ: hp����


    void Start()
    {
        if (player == null)
            player = GetComponentInParent<Player>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // ���� �浹 �� �÷��̾��� hp�� ���ҽ�Ŵ
            player.TakeDamage(20);

        }
    }
}
