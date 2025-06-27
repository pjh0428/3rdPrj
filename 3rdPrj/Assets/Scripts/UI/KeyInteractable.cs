// Assets/Scripts/Interact/KeyInteractable.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyInteractable : MonoBehaviour, IInteractable
{
    private void Reset()
    {
        // Collider를 트리거로 자동 세팅
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void Interact()
    {
        // 키 습득 로직
        KeyUIController.Instance.OnKeyCollected();
        // (선택) 사운드·파티클 재생...

        Destroy(gameObject);
    }
}
