using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [Header("Full-screen black panel")]
    [SerializeField] private Image fadePanel;      // Canvas 위에 풀스크린 Image
    [SerializeField, Min(0.1f)] private float duration = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // 시작 시 완전 투명(보이지 않음)
        fadePanel.gameObject.SetActive(true);
        var c = fadePanel.color;
        c.a = 0f;
        fadePanel.color = c;
    }

    /// <summary>0→1 로 페이드아웃(검정) 콜백 이후 onComplete 호출</summary>
    public void FadeOut(Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(0f, 1f, onComplete));
    }

    /// <summary>1→0 로 페이드인(밝아짐) 콜백 이후 onComplete 호출</summary>
    public void FadeIn(Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(1f, 0f, onComplete));
    }

    private IEnumerator Fade(float from, float to, Action onComplete)
    {
        fadePanel.gameObject.SetActive(true);
        float t = 0f;
        Color c = fadePanel.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            fadePanel.color = c;
            yield return null;
        }
        c.a = to;
        fadePanel.color = c;
        if (to <= 0f) fadePanel.gameObject.SetActive(false);
        onComplete?.Invoke();
    }
}
