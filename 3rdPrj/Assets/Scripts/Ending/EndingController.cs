using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class EndingController : MonoBehaviour
{
    [Header("스토리 텍스트 목록")]
    [TextArea(3, 6)]
    [SerializeField] private List<string> messages;

    [Header("UI 참조")]
    [SerializeField] private Text messageText;

    [Header("타자기 효과 속도")]
    [SerializeField, Min(0f)] private float typeDelay = 0.05f;

    [Header("페이드 설정")]
    [SerializeField, Min(0.1f)] private float fadeDuration = 0.5f;

    [SerializeField] private string nextSceneName = "LobbyScene";

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typeCoroutine;

    private void Start()
    {
        Time.timeScale = 1f;
        // 씬 시작 시 페이드인 후 시퀀스 실행
        ScreenFader.Instance.FadeIn(() =>
        {
            BeginSequence();
        });
    }

    private void BeginSequence()
    {
        index = 0;
        messageText.text = "";
        typeCoroutine = StartCoroutine(TypeText(messages[index]));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // 타이핑 중엔 즉시 전체 출력
                StopCoroutine(typeCoroutine);
                messageText.text = messages[index];
                isTyping = false;
            }
            else
            {
                // 다 출력된 후엔 다음 메시지로
                if (index < messages.Count - 1)
                    StartCoroutine(NextMessage());
                else
                    StartCoroutine(EndAndLoad());
            }
        }
    }

    private IEnumerator NextMessage()
    {
        // 짧은 페이드아웃 딜레이 (선택)
        yield return new WaitForSeconds(fadeDuration);

        index++;
        messageText.text = "";
        typeCoroutine = StartCoroutine(TypeText(messages[index]));
    }

    private IEnumerator EndAndLoad()
    {
        // 모든 메시지 끝 → 씬 페이드아웃
        ScreenFader.Instance.FadeOut(() =>
        {
            SceneManager.LoadScene(nextSceneName);
        });
        yield break;
    }

    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        foreach (char c in fullText)
        {
            messageText.text += c;
            yield return new WaitForSeconds(typeDelay);
        }
        isTyping = false;
    }
}
