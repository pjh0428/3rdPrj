using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [SerializeField] private Image fadePanel;       // FadePanel의 Image 컴포넌트
    [SerializeField] private float fadeDuration = 2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        // 시작 시 비활성 상태 보장
        fadePanel.gameObject.SetActive(false);
    }

    /// <summary>게임 클리어 시 호출해서 천천히 검정으로 암전</summary>
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
        // (원하면 여기서 씬 전환 또는 결과 화면 로드)
    }
}
