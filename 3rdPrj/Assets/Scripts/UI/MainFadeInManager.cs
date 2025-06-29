using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainFadeInManager : MonoBehaviour
{
    [Header("Fade-In Settings")]
    [SerializeField] private Image fadePanel;          // Canvas ���� Ǯ��ũ�� Image
    [SerializeField, Min(0.1f)] private float fadeDuration = 2f;

    private void Awake()
    {
        // �� �г��� ���� ���� ���� ���·� ���̰� ����
        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;
        c.a = 1f;
        fadePanel.color = c;
    }

    private void Start()
    {
        // �� Start���� ���̵��� �ڷ�ƾ ����
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadePanel.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            // ���İ��� 1��0���� ����
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }
        // ������ ���������� �г� �����
        c.a = 0f;
        fadePanel.color = c;
        fadePanel.gameObject.SetActive(false);
    }
}
