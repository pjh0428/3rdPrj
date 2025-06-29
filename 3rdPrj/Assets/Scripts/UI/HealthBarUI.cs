using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Player player;   // �巡���ؼ� ����
    [SerializeField] private Image fillImage; // HealthBarFill ������Ʈ�� Image

    // Inspector���� ���� ���� ������ �� �ֵ��� Color �ʵ�� ����
    [Header("Health Bar Colors")]
    [SerializeField] private Color highColor = new Color(0f, 1f, 0f);
    [SerializeField] private Color midColor = new Color(1f, 1f, 0f);
    [SerializeField] private Color lowColor = new Color(1f, 0f, 0f);

    [Header("Smoothing")]
    [Tooltip("fillAmount�� ��ǥġ�� ������ ������ �ε巴�� ������ �ӵ�")]
    [SerializeField, Min(0f)] private float smoothSpeed = 5f;

    [Header("Damage Overlay")]
    [SerializeField] private Image bloodOverlay;      // �߰�: BloodOverlay Image
    [SerializeField, Range(0f, 1f)] private float overlayAlpha = 1f; // �ִ� ����
    [SerializeField, Range(0f, 1f)] private float minOverlayAlpha = 0.2f;  // 60% HP�� �� ���� ����


    private float maxHP;
    private float targetRatio;

    IEnumerator Start()
    {
        // �� ������ ����ؼ� Player.Start()�� ����ǵ���
        yield return null;

        if (player == null)
            player = Object.FindFirstObjectByType<Player>();

        maxHP = player.CurrentHP;
        targetRatio = 1f;
        fillImage.fillAmount = 1f;
    }

    void Update()
    {
        // �� ������ ��ǥ ���� ����
        float ratio = (maxHP > 0f) ? player.CurrentHP / maxHP : 0f;
        targetRatio = Mathf.Clamp01(ratio);

        // �ε巯�� ����
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetRatio,
            Time.deltaTime * smoothSpeed
        );

        // ������ ���� ���� ���� (������ ������ �ص� ���� targetRatio�� �ص� �������)
        float current = fillImage.fillAmount;
        if (current > 0.61f) fillImage.color = highColor;
        else if (current > 0.21f) fillImage.color = midColor;
        else fillImage.color = lowColor;

        // �� �������� ó�� (�ϳ��� �������)
        if (ratio <= 0.6f)
        {
            // 60% ���� ������ 0~1�� ����ȭ (ratio=0.6��0, ratio=0��1)
            float norm = Mathf.InverseLerp(0.6f, 0f, ratio);

            // ����ȭ ���� overlayAlpha�� ���ؼ� �⺻ ���� ���
            float baseAlpha = Mathf.Lerp(minOverlayAlpha, overlayAlpha, norm);

            // �޽� �ֱ�
            float pulse = (Mathf.Sin(Time.time * 5f) + 1f) * 0.5f;

            // ���� ����
            float a = baseAlpha * pulse;
            bloodOverlay.color = new Color(1f, 1f, 1f, a);
        }
        else
        {
            bloodOverlay.color = Color.clear;
        }
    }

}
