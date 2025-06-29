using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [Header("Full-screen black panel")]
    [SerializeField] private Image fadePanel;
    [SerializeField, Min(0.1f)] private float defaultDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        else { Destroy(gameObject); return; }

        // �г� ������Ʈ�� �׻� Ȱ��ȭ, Image ������Ʈ�� ��
        fadePanel.enabled = false;
        Color c = fadePanel.color;
        c.a = 0f;
        fadePanel.color = c;
    }

    /// <summary>
    /// ���̵�ƿ�: �������� ����. customDuration ������ �� defaultDuration ���.
    /// </summary>
    public void FadeOut(Action onComplete = null, float? customDuration = null)
    {
        StopAllCoroutines();
        float dur = customDuration.HasValue ? customDuration.Value : defaultDuration;
        StartCoroutine(Fade(0f, 1f, dur, onComplete));
    }

    /// <summary>
    /// ���̵���: �������� �����. customDuration ������ �� defaultDuration ���.
    /// </summary>
    public void FadeIn(Action onComplete = null, float? customDuration = null)
    {
        StopAllCoroutines();
        float dur = customDuration.HasValue ? customDuration.Value : defaultDuration;
        StartCoroutine(Fade(1f, 0f, dur, onComplete));
    }

    private IEnumerator Fade(float from, float to, float dur, Action onComplete)
    {
        fadePanel.enabled = true;
        float t = 0f;
        Color c = fadePanel.color;

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(from, to, t / dur);
            fadePanel.color = c;
            yield return null;
        }

        c.a = to;
        fadePanel.color = c;
        if (to <= 0f) fadePanel.enabled = false;

        onComplete?.Invoke();
    }
}
