using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [TextArea(3, 8)]
    [SerializeField] private string message;


    [Header("Key UI")]
    [SerializeField] private bool showKeyAfterClose = false;  // TTBook2���� üũ

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        // showKeyUI �� ���� ��쿡�� KeyUIPanel �ѱ�
        UIManager.Instance.ShowNotification(message, showKeyAfterClose);

        hasTriggered = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // �÷��̾ å���� ������ ����� �ٽ� Ʈ���� �����ϵ��� ����
        hasTriggered = false;
        Debug.Log("[TutorialTrigger] ����");
    }
}