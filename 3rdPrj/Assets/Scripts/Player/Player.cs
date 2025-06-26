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
        Debug.Log($"�÷��̾ {damage}�� ��������");
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("�÷��̾� ���");
        animator.SetTrigger("Dying");
    }

}
