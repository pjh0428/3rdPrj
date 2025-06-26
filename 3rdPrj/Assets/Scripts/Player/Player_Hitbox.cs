using UnityEngine;

public class Player_Hitbox : MonoBehaviour
{
    public Player player;


    void Start()
    {
        if (player == null)
            player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            player.TakeDamage(20);

        }
    }
}
