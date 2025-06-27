using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [SerializeField] private Image fadePanel;       // FadePanel�� Image ������Ʈ
    [SerializeField] private float fadeDuration = 2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        // ���� �� ��Ȱ�� ���� ����
        fadePanel.gameObject.SetActive(false);
    }

    /// <summary>���� Ŭ���� �� ȣ���ؼ� õõ�� �������� ����</summary>
    public void ClearGame()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }
        // (���ϸ� ���⼭ �� ��ȯ �Ǵ� ��� ȭ�� �ε�)
    }
}
