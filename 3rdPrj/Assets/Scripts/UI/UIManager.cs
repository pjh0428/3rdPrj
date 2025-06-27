using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Notification UI")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Text notificationText;

    private bool isShowing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            notificationPanel.SetActive(false);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!isShowing)
            return;

        // 새 Input System에서 E 키 눌림 감지
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            HideNotification();
        }
    }

    /// <summary>
    /// 메시지를 띄우고, 성공적으로 띄웠다면 true, 이미 떠 있다면 false
    /// </summary>
    public bool ShowNotification(string message)
    {
        if (isShowing)
            return false;               // 이미 떠 있으면 아무것도 안 하고 false

        notificationText.text = message;
        notificationPanel.SetActive(true);
        isShowing = true;
        return true;                    // 정상적으로 띄우고 true
    }

    public void HideNotification()
    {
        notificationPanel.SetActive(false);
        isShowing = false;
    }
}
