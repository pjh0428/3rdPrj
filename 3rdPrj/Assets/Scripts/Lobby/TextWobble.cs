using UnityEngine;
using TMPro;

public class TextWobble : MonoBehaviour
{
    public TMP_Text text;

    [Header("Alpha ����")]
    [Range(0f, 1f)] public float minAlpha = 0.3f;
    [Range(0f, 1f)] public float maxAlpha = 1f;

    [Header("��ȭ �ӵ�")]
    public float flickerSpeed = 2f;

    private float noiseSeed;

    void Awake()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();

        noiseSeed = Random.Range(0f, 100f);
    }

    void Update()
    {
        float time = Time.time * flickerSpeed + noiseSeed;

        // �ε巯�� ������ ��� alpha
        float flicker = Mathf.PerlinNoise(time, 0f); // 0~1 ���� �ε巯�� ��
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, flicker);

        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}
