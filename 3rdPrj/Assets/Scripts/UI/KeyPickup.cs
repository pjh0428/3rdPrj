using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // UI업데이트
        KeyUIController.Instance.OnKeyCollected();
        // (선택) 수집 효과 재생 (사운드, 파티클 등)
        // 예: AudioSource.PlayClipAtPoint(pickupSfx, transform.position);

        // 자기 자신 제거
        Destroy(gameObject);
    }
}
