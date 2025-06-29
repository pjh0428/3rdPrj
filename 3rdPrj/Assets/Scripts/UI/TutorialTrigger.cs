using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [TextArea(3, 8)]
    [SerializeField] private string message;


    [Header("Key UI")]
    [SerializeField] private bool showKeyAfterClose = false;  // TTBook2에만 체크

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        // showKeyUI 가 켜진 경우에만 KeyUIPanel 켜기
        UIManager.Instance.ShowNotification(message, showKeyAfterClose);

        hasTriggered = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // 플레이어가 책에서 완전히 벗어나면 다시 트리거 가능하도록 리셋
        hasTriggered = false;
        Debug.Log("[TutorialTrigger] 리셋");
    }
}