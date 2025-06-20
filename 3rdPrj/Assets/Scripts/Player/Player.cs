using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("�⺻ ����")]
    [SerializeField] private float MaxHP = 100f;
    [SerializeField] private float CurrentHP = 100f;


    void Start()
    {
        CurrentHP = MaxHP;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        Debug.Log($"�÷��̾ {damage}�� ��������");
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            //Die();
        }
    }

    public void Die()
    {
        Debug.Log("�÷��̾� ���");
        // �ִϸ��̼� �߰�
      
    }

}
