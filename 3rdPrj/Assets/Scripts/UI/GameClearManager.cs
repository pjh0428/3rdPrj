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
    [SerializeField] private string clearSceneName = "ClearScene"; // 페이드 후 로드할 씬 이름

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // 시작 시 완전 투명 상태로 설정
        fadePanel.gameObject.SetActive(true);
        var col = fadePanel.color;
        col.a = 0f;
        fadePanel.color = col;
    }

    /// <summary>게임 클리어 시 호출: 게임 로직 멈추고 페이드아웃 & 씬 전환</summary>
    public void ClearGame()
    {
        // 1) 게임 타임 스케일 정지
        Time.timeScale = 0f;

        // 2) 플레이어 컨트롤러 비활성
        var playerCtrl = Object.FindFirstObjectByType<PlayerController>();
        if (playerCtrl != null) playerCtrl.enabled = false;
        var aimCtrl = Object.FindFirstObjectByType<AimController>();
        if (aimCtrl != null) aimCtrl.enabled = false;

        // 3) 페이드아웃 시작
        StartCoroutine(FadeOutAndLoad());
    }

    private IEnumerator FadeOutAndLoad()
    {
        // 패널 활성화
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

        // 씬 전환
        SceneManager.LoadScene(clearSceneName);
    }
}
