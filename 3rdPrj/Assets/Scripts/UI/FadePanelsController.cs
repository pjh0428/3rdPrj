using UnityEngine;
using System.Collections;

public class FadePanelsController : MonoBehaviour
{
    [Header("모서리 패널들")]
    [SerializeField] private RectTransform fadeLeft;
    [SerializeField] private RectTransform fadeRight;
    [SerializeField] private RectTransform fadeTop;
    [SerializeField] private RectTransform fadeBottom;

    [Header("페이드 설정")]
    [SerializeField, Min(0.1f)] private float duration = 1f;

    private float fullWidth;
    private float fullHeight;
    private float halfWidth;
    private float halfHeight;

    private void Awake()
    {
        // 캔버스 크기 계산
        var canvasRect = fadeLeft.GetComponentInParent<Canvas>()
                                 .GetComponent<RectTransform>().rect;
        fullWidth = canvasRect.width;
        fullHeight = canvasRect.height;
        halfWidth = fullWidth / 2f;
        halfHeight = fullHeight / 2f;

        // **기본 상태**는 열린 상태(판넬 크기 0)로
        SetPanels(0f, 0f);
    }

    /// <summary>인트로 씬에서만 호출해요: 중앙→사방으로 열기</summary>
    public void StartUnfade()
    {
        StopAllCoroutines();
        // 중앙 크기로 세팅 후
        SetPanels(halfWidth, halfHeight);
        StartCoroutine(FadeOutwards());
    }

    /// <summary>로비 씬(엔터)에만 호출: 사방→중앙으로 닫기</summary>
    public void StartFade()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInwards());
    }

    private IEnumerator FadeOutwards()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float r = Mathf.Clamp01(t / duration);
            float w = Mathf.Lerp(halfWidth, 0f, r);
            float h = Mathf.Lerp(halfHeight, 0f, r);
            SetPanels(w, h);
            yield return null;
        }
        SetPanels(0f, 0f);
    }

    private IEnumerator FadeInwards()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float r = Mathf.Clamp01(t / duration);
            float w = Mathf.Lerp(0f, halfWidth, r);
            float h = Mathf.Lerp(0f, halfHeight, r);
            SetPanels(w, h);
            yield return null;
        }
        SetPanels(halfWidth, halfHeight);
    }

    private void SetPanels(float w, float h)
    {
        fadeLeft.sizeDelta = new Vector2(w, fullHeight);
        fadeRight.sizeDelta = new Vector2(w, fullHeight);
        fadeTop.sizeDelta = new Vector2(fullWidth, h);
        fadeBottom.sizeDelta = new Vector2(fullWidth, h);
    }

    public float Duration => duration;
}
