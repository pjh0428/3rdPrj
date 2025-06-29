using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Player player;   // 드래그해서 연결
    [SerializeField] private Image fillImage; // HealthBarFill 오브젝트의 Image

    // Inspector에서 직접 색을 지정할 수 있도록 Color 필드로 노출
    [Header("Health Bar Colors")]
    [SerializeField] private Color highColor = new Color(0f, 1f, 0f);
    [SerializeField] private Color midColor = new Color(1f, 1f, 0f);
    [SerializeField] private Color lowColor = new Color(1f, 0f, 0f);

    [Header("Smoothing")]
    [Tooltip("fillAmount가 목표치에 도달할 때까지 부드럽게 보정할 속도")]
    [SerializeField, Min(0f)] private float smoothSpeed = 5f;

    [Header("Damage Overlay")]
    [SerializeField] private Image bloodOverlay;      // 추가: BloodOverlay Image
    [SerializeField, Range(0f, 1f)] private float overlayAlpha = 1f; // 최대 알파
    [SerializeField, Range(0f, 1f)] private float minOverlayAlpha = 0.2f;  // 60% HP일 때 시작 알파


    private float maxHP;
    private float targetRatio;

    IEnumerator Start()
    {
        // 한 프레임 대기해서 Player.Start()가 실행되도록
        yield return null;

        if (player == null)
            player = Object.FindFirstObjectByType<Player>();

        maxHP = player.CurrentHP;
        targetRatio = 1f;
        fillImage.fillAmount = 1f;
    }

    void Update()
    {
        // 매 프레임 목표 비율 갱신
        float ratio = (maxHP > 0f) ? player.CurrentHP / maxHP : 0f;
        targetRatio = Mathf.Clamp01(ratio);

        // 부드러운 보정
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetRatio,
            Time.deltaTime * smoothSpeed
        );

        // 비율에 따라 색상 변경 (보정된 비율로 해도 좋고 targetRatio로 해도 상관없음)
        float current = fillImage.fillAmount;
        if (current > 0.61f) fillImage.color = highColor;
        else if (current > 0.21f) fillImage.color = midColor;
        else fillImage.color = lowColor;

        // 피 오버레이 처리 (하나의 블록으로)
        if (ratio <= 0.6f)
        {
            // 60% 이하 구간을 0~1로 정규화 (ratio=0.6→0, ratio=0→1)
            float norm = Mathf.InverseLerp(0.6f, 0f, ratio);

            // 정규화 값에 overlayAlpha를 곱해서 기본 알파 계산
            float baseAlpha = Mathf.Lerp(minOverlayAlpha, overlayAlpha, norm);

            // 펄스 주기
            float pulse = (Mathf.Sin(Time.time * 5f) + 1f) * 0.5f;

            // 최종 알파
            float a = baseAlpha * pulse;
            bloodOverlay.color = new Color(1f, 1f, 1f, a);
        }
        else
        {
            bloodOverlay.color = Color.clear;
        }
    }

}
