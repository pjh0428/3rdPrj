// Assets/Scripts/Player/PlayerInteraction.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Camera playerCamera;       // 플레이어 카메라
    [SerializeField] private float interactRange = 3f;

    [Header("UI")]
    [SerializeField] private GameObject interactPrompt; // InteractPrompt 오브젝트

    private IInteractable currentHover;

    void Update()
    {
        UpdateHover();
        HandleInput();
    }

    private void UpdateHover()
    {
        // 화면 중앙에서 레이CAST
        var ray = playerCamera.ScreenPointToRay(
            new Vector2(Screen.width / 2f, Screen.height / 2f)
        );
        if (Physics.Raycast(ray, out var hit, interactRange))
        {
            currentHover = hit.collider.GetComponent<IInteractable>();
        }
        else currentHover = null;

        // 프롬프트 보이기/숨기기
        interactPrompt.SetActive(currentHover != null);
    }

    private void HandleInput()
    {
        if (currentHover != null &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentHover.Interact();
            currentHover = null;
            interactPrompt.SetActive(false);
        }
    }
}
