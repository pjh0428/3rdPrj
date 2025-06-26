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
            Debug.Log("[TutorialTrigger] �޽��� ��� ����");
            hasTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // �÷��̾ å���� ������ ����� �ٽ� Ʈ���� �����ϵ��� ����
        hasTriggered = false;
        Debug.Log("[TutorialTrigger] ����");
    }
}