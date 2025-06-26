// Assets/Scripts/Interact/KeyInteractable.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyInteractable : MonoBehaviour, IInteractable
{
    private void Reset()
    {
        // Collider�� Ʈ���ŷ� �ڵ� ����
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void Interact()
    {
        // Ű ���� ����
        KeyUIController.Instance.OnKeyCollected();
        // (����) ���塤��ƼŬ ���...

        Destroy(gameObject);
    }
}
