using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorInteractable : MonoBehaviour, IInteractable
{
    private void Reset()
    {
        // Ʈ���� �ݶ��̴��� �ڵ����� �ٵ���
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void Interact()
    {
        // ���� ���� ��
        if (KeyUIController.Instance.CollectedKeys < KeyUIController.Instance.TotalKeys)
        {
            UIManager.Instance.ShowNotification("���� ������ �ʴ´�", false, true);
        }
        else
        {
            GameClearManager.Instance.ClearGame();
        }
    }
}
