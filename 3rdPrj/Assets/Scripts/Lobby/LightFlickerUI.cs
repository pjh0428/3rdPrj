using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LightFlickerUI : MonoBehaviour
{
    [Header("Flicker Settings")]
    [Tooltip("�� ����Ŭ(��ο�����)�� �ɸ��� �ð�(��)")]
    [SerializeField, Min(0.01f)] private float flickDuration = 1f;

    [Tooltip("�ּ� ��� ���� ����(0~1)")]
    [SerializeField, Range(0f, 1f)] private float globalMinBrightness = 0.5f;

    [Tooltip("�ִ� ���(�׻� ����)")]
    [SerializeField, Range(1f, 2f)] private float maxBrightness = 1.2f;

    [Tooltip("�ּ� ��⸦ ���� �� maxBrightness���� �� ��(��: 0.2�� �θ� max-0.2 ������ ����)")]
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
        // 1) Ÿ�̸� ����
        timer += Time.deltaTime;
        float t = timer / flickDuration;

        if (t >= 1f)
        {
            // ����Ŭ �� �� ���� ����Ŭ �غ�
            timer = 0f;
            PickNewMin();
            t = 0f;
        }

        // 2) ��ο�(currentMin)�����(maxBrightness) ���� ����
        float brightness = Mathf.Lerp(currentMin, maxBrightness, t);

        // 3) ���� �÷��� ���� ���� (���ĸ� ����)
        img.color = new Color(
            original.r * brightness,
            original.g * brightness,
            original.b * brightness,
            original.a
        );
    }

    /// <summary>
    /// globalMinBrightness ~ (maxBrightness - minMaxMargin) ��������
    /// ���� ����Ŭ�� �ּ� ��⸦ �������� �̴´�.
    /// </summary>
    private void PickNewMin()
    {
        float maxMin = Mathf.Max(globalMinBrightness, maxBrightness - minMaxMargin);
        currentMin = Random.Range(globalMinBrightness, maxMin);
    }

    // Inspector���� flickDuration ���� �ٲٸ� �ٷ� �ݿ��ǵ���
    private void OnValidate()
    {
        flickDuration = Mathf.Max(0.01f, flickDuration);
        globalMinBrightness = Mathf.Clamp01(globalMinBrightness);
        maxBrightness = Mathf.Max(1f, maxBrightness);
        minMaxMargin = Mathf.Clamp(minMaxMargin, 0f, maxBrightness - globalMinBrightness);
    }
}
