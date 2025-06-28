using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("기본 스탯")]
    [SerializeField] private float MaxHP = 100f;
    [SerializeField] public float CurrentHP = 100f;

    [Header("발소리")]
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
        if (audioSource == null || (footsteps.Length == 0) || audioSource == null)
            return;
        AudioClip clip = footsteps[footstepIndex];

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
