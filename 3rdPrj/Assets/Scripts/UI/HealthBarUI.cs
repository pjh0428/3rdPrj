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
        if (current > 0.6f) fillImage.color = highColor;
        else if (current > 0.2f) fillImage.color = midColor;
        else fillImage.color = lowColor;
    }
}
