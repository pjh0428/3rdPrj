using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [TextArea(3, 8)]
    [SerializeField] private string message;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        bool shown = UIManager.Instance.ShowNotification(message);
        if (shown)
        {
            Debug.Log("[TutorialTrigger] 메시지 띄움 성공");
            hasTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // 플레이어가 책에서 완전히 벗어나면 다시 트리거 가능하도록 리셋
        hasTriggered = false;
        Debug.Log("[TutorialTrigger] 리셋");
    }
}