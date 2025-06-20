using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("기본 스탯")]
    [SerializeField] private float MaxHP = 100f;
    [SerializeField] private float CurrentHP = 100f;


    void Start()
    {
        CurrentHP = MaxHP;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        Debug.Log($"플레이어가 {damage}의 피해입음");
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            //Die();
        }
    }

    public void Die()
    {
        Debug.Log("플레이어 사망");
        // 애니메이션 추가
      
    }

}
