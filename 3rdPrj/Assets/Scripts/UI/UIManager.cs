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

        // �� Input System���� E Ű ���� ����
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            HideNotification();
        }
    }

    /// <summary>
    /// �޽����� ����, ���������� ����ٸ� true, �̹� �� �ִٸ� false
    /// </summary>
    public bool ShowNotification(string message)
    {
        if (isShowing)
            return false;               // �̹� �� ������ �ƹ��͵� �� �ϰ� false

        notificationText.text = message;
        notificationPanel.SetActive(true);
        isShowing = true;
        return true;                    // ���������� ���� true
    }

    public void HideNotification()
    {
        notificationPanel.SetActive(false);
        isShowing = false;
    }
}
