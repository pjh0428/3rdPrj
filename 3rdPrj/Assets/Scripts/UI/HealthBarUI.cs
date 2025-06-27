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
        if (current > 0.6f) fillImage.color = highColor;
        else if (current > 0.2f) fillImage.color = midColor;
        else fillImage.color = lowColor;
    }
}
