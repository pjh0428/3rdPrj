using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum BodyPart { Body, Head }
    public EnemyFSM zombie;

    public float demageMultiplier;

    int bulletLayer;

    void Start()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == bulletLayer)
        {
            if (other.CompareTag("Head"))
                demageMultiplier = 5;
            if (other.CompareTag("Body"))
                demageMultiplier = 1;
            
            int baseDamage = other.GetComponent<Bullet>().damaged;
            int finalDamage = Mathf.RoundToInt(baseDamage * demageMultiplier);
            zombie.HitEnemy(finalDamage);

            Debug.Log("collider");
        }

    }
    
    

}
