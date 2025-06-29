using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private Image fadePanel;       // Fullscreen black panel Image
    [SerializeField, Min(0.1f)] private float fadeDuration = 2f;
    [SerializeField] private string clearSceneName = "ClearScene"; // ���̵� �� �ε��� �� �̸�

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // ���� �� ���� ���� ���·� ����
        fadePanel.gameObject.SetActive(true);
        var col = fadePanel.color;
        col.a = 0f;
        fadePanel.color = col;
    }

    /// <summary>���� Ŭ���� �� ȣ��: ���� ���� ���߰� ���̵�ƿ� & �� ��ȯ</summary>
    public void ClearGame()
    {
        // 1) ���� Ÿ�� ������ ����
        Time.timeScale = 0f;

        // 2) �÷��̾� ��Ʈ�ѷ� ��Ȱ��
        var playerCtrl = Object.FindFirstObjectByType<PlayerController>();
        if (playerCtrl != null) playerCtrl.enabled = false;
        var aimCtrl = Object.FindFirstObjectByType<AimController>();
        if (aimCtrl != null) aimCtrl.enabled = false;

        // 3) ���̵�ƿ� ����
        StartCoroutine(FadeOutAndLoad());
    }

    private IEnumerator FadeOutAndLoad()
    {
        // �г� Ȱ��ȭ
        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        // �� ��ȯ
        SceneManager.LoadScene(clearSceneName);
    }
}
