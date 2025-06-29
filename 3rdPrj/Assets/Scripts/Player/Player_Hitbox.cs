using UnityEngine;
using System.Collections;

public class Player_Hitbox : MonoBehaviour
{
    public Player player;
    public float invincibilityTime = 1.0f; // 무적 지속 시간

    private Collider hitboxCollider;

    void Start()
    {
        if (player == null)
            player = GetComponentInParent<Player>();

        hitboxCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            player.TakeDamage(10);
            UIShake.Instance.Shake(0.2f, 10f);
            StartCoroutine(DisableHitbox());
        }
    }

    private IEnumerator DisableHitbox()
    {
        hitboxCollider.enabled = false;
        yield return new WaitForSeconds(invincibilityTime);
        hitboxCollider.enabled = true;
    }
}
