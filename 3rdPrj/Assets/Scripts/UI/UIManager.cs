using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Notification UI")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Text notificationText;

    private Image panelImage;
    private Color originalPanelColor;

    private bool isShowing = false;
    private bool showKeyAfterClose = false;

    private Coroutine autoHideRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            panelImage = notificationPanel.GetComponent<Image>();
            originalPanelColor = panelImage.color;

            notificationPanel.SetActive(false);
        }
        else Destroy(gameObject);
    }

    // �޽����� ����, ���������� ����ٸ� true, �̹� �� �ִٸ� false
    public bool ShowNotification(string message, bool showKeyAfterClose = false, bool transparentBackground = false)
    {
        if (isShowing) return false;

        // ��� ���� ó��
        if (transparentBackground)
        {
            panelImage.color = new Color(originalPanelColor.r, originalPanelColor.g, originalPanelColor.b, 0f);
        }
        else
        {
            panelImage.color = originalPanelColor;
        }

        notificationText.text = message;
        notificationPanel.SetActive(true);
        isShowing = true;
        this.showKeyAfterClose = showKeyAfterClose;

        // �ڵ� ���� ���� (���� ��� �޽�����)
        if (transparentBackground)
        {
            if (autoHideRoutine != null)
                StopCoroutine(autoHideRoutine);
            autoHideRoutine = StartCoroutine(AutoHide(1f)); // 1�� �� �ڵ� ����
        }

        return true;
    }

    // �޽��� �ڵ� ����
    private IEnumerator AutoHide(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideNotification();
    }

    public void HideNotification()
    {
        if (!isShowing) return;

        notificationPanel.SetActive(false);
        isShowing = false;

        // ���� �� �÷��װ� ���� ������ KeyUI ����
        if (showKeyAfterClose)
        {
            KeyUIController.Instance.ShowKeyUI();
            showKeyAfterClose = false;
        }

        // �ڵ� ���� ��ƾ ����
        if (autoHideRoutine != null)
        {
            StopCoroutine(autoHideRoutine);
            autoHideRoutine = null;
        }

    }

    private void Update()
    {
        if (!isShowing) return;

        // �� Input System���� E Ű ���� ����
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            HideNotification();
        }
    }
}
