using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // UI������Ʈ
        KeyUIController.Instance.OnKeyCollected();
        // (����) ���� ȿ�� ��� (����, ��ƼŬ ��)
        // ��: AudioSource.PlayClipAtPoint(pickupSfx, transform.position);

        // �ڱ� �ڽ� ����
        Destroy(gameObject);
    }
}
