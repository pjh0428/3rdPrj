using UnityEngine;
using System.Collections;

public class FadePanelsController : MonoBehaviour
{
    [Header("�𼭸� �гε�")]
    [SerializeField] private RectTransform fadeLeft;
    [SerializeField] private RectTransform fadeRight;
    [SerializeField] private RectTransform fadeTop;
    [SerializeField] private RectTransform fadeBottom;

    [Header("���̵� ����")]
    [SerializeField, Min(0.1f)] private float duration = 1f;

    private float fullWidth;
    private float fullHeight;
    private float halfWidth;
    private float halfHeight;

    private void Awake()
    {
        // ĵ���� ũ�� ���
        var canvasRect = fadeLeft.GetComponentInParent<Canvas>()
                                 .GetComponent<RectTransform>().rect;
        fullWidth = canvasRect.width;
        fullHeight = canvasRect.height;
        halfWidth = fullWidth / 2f;
        halfHeight = fullHeight / 2f;

        // **�⺻ ����**�� ���� ����(�ǳ� ũ�� 0)��
        SetPanels(0f, 0f);
    }

    /// <summary>��Ʈ�� �������� ȣ���ؿ�: �߾ӡ������� ����</summary>
    public void StartUnfade()
    {
        StopAllCoroutines();
        // �߾� ũ��� ���� ��
        SetPanels(halfWidth, halfHeight);
        StartCoroutine(FadeOutwards());
    }

    /// <summary>�κ� ��(����)���� ȣ��: �����߾����� �ݱ�</summary>
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
