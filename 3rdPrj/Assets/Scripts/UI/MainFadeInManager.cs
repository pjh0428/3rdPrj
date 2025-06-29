using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainFadeInManager : MonoBehaviour
{
    [Header("Fade-In Settings")]
    [SerializeField] private Image fadePanel;          // Canvas 위에 풀스크린 Image
    [SerializeField, Min(0.1f)] private float fadeDuration = 2f;

    private void Awake()
    {
        // ① 패널을 먼저 완전 검정 상태로 보이게 세팅
        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;
        c.a = 1f;
        fadePanel.color = c;
    }

    private void Start()
    {
        // ② Start에서 페이드인 코루틴 실행
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadePanel.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            // 알파값을 1→0으로 보간
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }
        // 완전히 투명해지면 패널 숨기기
        c.a = 0f;
        fadePanel.color = c;
        fadePanel.gameObject.SetActive(false);
    }
}
