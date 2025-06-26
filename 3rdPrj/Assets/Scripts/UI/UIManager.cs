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

    // 메시지를 띄우고, 성공적으로 띄웠다면 true, 이미 떠 있다면 false
    public bool ShowNotification(string message, bool showKeyAfterClose = false, bool transparentBackground = false)
    {
        if (isShowing) return false;

        // 배경 투명 처리
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

        // 자동 숨김 설정 (투명 배경 메시지용)
        if (transparentBackground)
        {
            if (autoHideRoutine != null)
                StopCoroutine(autoHideRoutine);
            autoHideRoutine = StartCoroutine(AutoHide(1f)); // 1초 후 자동 숨김
        }

        return true;
    }

    // 메시지 자동 숨김
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

        // 닫힐 때 플래그가 켜져 있으면 KeyUI 열기
        if (showKeyAfterClose)
        {
            KeyUIController.Instance.ShowKeyUI();
            showKeyAfterClose = false;
        }

        // 자동 숨김 루틴 정리
        if (autoHideRoutine != null)
        {
            StopCoroutine(autoHideRoutine);
            autoHideRoutine = null;
        }

    }

    private void Update()
    {
        if (!isShowing) return;

        // 새 Input System에서 E 키 눌림 감지
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            HideNotification();
        }
    }
}
