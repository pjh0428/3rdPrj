using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("�⺻ ����")]
    [SerializeField] private float MaxHP = 100f;
    [SerializeField] public float CurrentHP = 100f;

    [Header("�߼Ҹ�")]
    public AudioClip[] footsteps;
  
    private int footstepIndex = 0;

    public bool isDead = false;
    private Animator animator;
    private AudioSource audioSource;



    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        CurrentHP = MaxHP;
    }

    public void PlayFootstepSound()
    {
        AudioClip clip = footsteps[footstepIndex];

        if (audioSource == null || (footsteps.Length == 0))         
            return;
     
        if(clip != null)
            audioSource.PlayOneShot(clip);
        footstepIndex++;
        if (footstepIndex >= footsteps.Length)
        {
            footstepIndex = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Dying");
    }

}
