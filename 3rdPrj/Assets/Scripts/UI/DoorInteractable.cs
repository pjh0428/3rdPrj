using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorInteractable : MonoBehaviour, IInteractable
{
    private void Reset()
    {
        // 트리거 콜라이더가 자동으로 붙도록
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void Interact()
    {
        // 열쇠 개수 비교
        if (KeyUIController.Instance.CollectedKeys < KeyUIController.Instance.TotalKeys)
        {
            UIManager.Instance.ShowNotification("문이 열리지 않는다", false, true);
        }
        else
        {
            GameClearManager.Instance.ClearGame();
        }
    }
}
