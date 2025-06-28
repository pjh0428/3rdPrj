using UnityEngine;
using System.Collections;

public class UIShake : MonoBehaviour
{
    public static UIShake Instance { get; private set; }

    [Tooltip("흔들 대상 RectTransform (Canvas나 특정 패널)")]
    [SerializeField] private RectTransform uiRoot;

    [Header("기본 설정")]
    [SerializeField, Min(0f)] private float defaultDuration = 0.2f;
    [SerializeField, Min(0f)] private float defaultMagnitude = 10f;

    private Vector2 originalPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (uiRoot == null) uiRoot = GetComponent<RectTransform>();
        originalPos = uiRoot.anchoredPosition;
    }

    /// <summary>
    /// UI를 흔듭니다.
    /// duration: 지속시간(초), magnitude: 흔들림 세기
    /// </summary>
    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(
            duration > 0f ? duration : defaultDuration,
            magnitude > 0f ? magnitude : defaultMagnitude
        ));
    }

    private IEnumerator ShakeCoroutine(float dur, float mag)
    {
        float elapsed = 0f;
        while (elapsed < dur)
        {
            // 화면 좌표(픽셀) 단위로 랜덤 오프셋
            float x = Random.Range(-1f, 1f) * mag;
            float y = Random.Range(-1f, 1f) * mag;
            uiRoot.anchoredPosition = originalPos + new Vector2(x, y);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        // 원위치 복귀
        uiRoot.anchoredPosition = originalPos;
    }
}
