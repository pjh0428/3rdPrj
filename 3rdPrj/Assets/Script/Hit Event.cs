using UnityEngine;

public class HitEvent : MonoBehaviour
{
    //에너미 스크립트 컴포트를 사용하기 위한 변수
    public EnemyFSM enemyFSM;

    public void PlayerHit()
    {
        enemyFSM.AttackAction();
        
    }

    public void PlayerHItEnd()
    {
        enemyFSM.AttackEnd();
    }
}
