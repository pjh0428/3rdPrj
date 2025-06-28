using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class IntroController : MonoBehaviour
{
    [Header("스토리 텍스트 목록")]
    [TextArea(3, 6)]           // 최소 3줄, 최대 6줄 에디터
    [SerializeField]
    private List<string> messages;
    [Header("그림(Sprite) 목록")]
    [SerializeField] private List<Sprite> pictures;

    [Header("UI 참조")]
    [SerializeField] private Image pictureImage;
    [SerializeField] private Text messageText;

    [Header("페이드 설정")]
    [SerializeField, Min(0.1f)] private float fadeDuration = 0.5f;
    [Header("타자기 효과 속도")]
    [SerializeField, Min(0f)] private float typeDelay = 0.05f;

    [SerializeField] private string nextSceneName = "LobbyScene";

    [SerializeField] private FadePanelsController fadeController;

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typeCoroutine;

    private void Start()
    {
        // 씬 시작 직후에 언페이드 효과
        fadeController.StartUnfade();

        // 첫 메시지와 그림 세팅
        messageText.text = "";
        pictureImage.sprite = pictures[index];
        SetImageAlpha(1f);

        // 첫 메시지 타이핑 시작
        typeCoroutine = StartCoroutine(TypeText(messages[index]));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // 타이핑 중에는 전부 출력만
                StopCoroutine(typeCoroutine);
                messageText.text = messages[index];
                isTyping = false;
            }
            else
            {
                // 완전 출력된 상태에서만 다음으로
                if (index < messages.Count - 1)
                    StartCoroutine(ShowNext());
                else
                    SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    private IEnumerator ShowNext()
    {
        // 그림 페이드 아웃
        yield return StartCoroutine(FadeImage(1f, 0f));

        // 다음 인덱스
        index++;
        pictureImage.sprite = pictures[index];

        // 그림 페이드 인
        yield return StartCoroutine(FadeImage(0f, 1f));

        // 메시지 초기화 후 타이핑
        messageText.text = "";
        typeCoroutine = StartCoroutine(TypeText(messages[index]));
    }

    private IEnumerator FadeImage(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetImageAlpha(Mathf.Lerp(from, to, t / fadeDuration));
            yield return null;
        }
        SetImageAlpha(to);
    }

    private void SetImageAlpha(float a)
    {
        var c = pictureImage.color;
        pictureImage.color = new Color(c.r, c.g, c.b, a);
    }

    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        for (int i = 0; i < fullText.Length; i++)
        {
            messageText.text += fullText[i];
            yield return new WaitForSeconds(typeDelay);
        }
        isTyping = false;
    }
}
