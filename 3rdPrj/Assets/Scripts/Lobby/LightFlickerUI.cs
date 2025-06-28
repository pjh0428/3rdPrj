using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LightFlickerUI : MonoBehaviour
{
    [Header("Flicker Settings")]
    [Tooltip("한 사이클(어두움→밝음)에 걸리는 시간(초)")]
    [SerializeField, Min(0.01f)] private float flickDuration = 1f;

    [Tooltip("최소 밝기 범위 하한(0~1)")]
    [SerializeField, Range(0f, 1f)] private float globalMinBrightness = 0.5f;

    [Tooltip("최대 밝기(항상 고정)")]
    [SerializeField, Range(1f, 2f)] private float maxBrightness = 1.2f;

    [Tooltip("최소 밝기를 뽑을 때 maxBrightness에서 뺄 값(예: 0.2로 두면 max-0.2 까지만 뽑음)")]
    [SerializeField, Range(0f, 1f)] private float minMaxMargin = 0.2f;

    private Image img;
    private Color original;
    private float timer;
    private float currentMin;

    private void Awake()
    {
        img = GetComponent<Image>();
        original = img.color;
        PickNewMin();
        timer = 0f;
    }

    private void Update()
    {
        // 1) 타이머 갱신
        timer += Time.deltaTime;
        float t = timer / flickDuration;

        if (t >= 1f)
        {
            // 사이클 끝 → 다음 사이클 준비
            timer = 0f;
            PickNewMin();
            t = 0f;
        }

        // 2) 어두움(currentMin)→밝음(maxBrightness) 선형 보간
        float brightness = Mathf.Lerp(currentMin, maxBrightness, t);

        // 3) 원래 컬러에 곱해 적용 (알파만 유지)
        img.color = new Color(
            original.r * brightness,
            original.g * brightness,
            original.b * brightness,
            original.a
        );
    }

    /// <summary>
    /// globalMinBrightness ~ (maxBrightness - minMaxMargin) 구간에서
    /// 다음 사이클용 최소 밝기를 랜덤으로 뽑는다.
    /// </summary>
    private void PickNewMin()
    {
        float maxMin = Mathf.Max(globalMinBrightness, maxBrightness - minMaxMargin);
        currentMin = Random.Range(globalMinBrightness, maxMin);
    }

    // Inspector에서 flickDuration 등을 바꾸면 바로 반영되도록
    private void OnValidate()
    {
        flickDuration = Mathf.Max(0.01f, flickDuration);
        globalMinBrightness = Mathf.Clamp01(globalMinBrightness);
        maxBrightness = Mathf.Max(1f, maxBrightness);
        minMaxMargin = Mathf.Clamp(minMaxMargin, 0f, maxBrightness - globalMinBrightness);
    }
}
