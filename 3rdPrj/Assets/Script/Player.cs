using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("�⺻ ����")]
    [SerializeField] private float MaxHP = 100f;
    [SerializeField] public float CurrentHP = 100f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        CurrentHP = MaxHP;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        Debug.Log($"플레이어 {damage} 데미지 받음");
        Debug.Log($"현재 체력: {CurrentHP}");
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("�÷��̾� ���");
        //animator.SetTrigger("Dying");
    }

}
