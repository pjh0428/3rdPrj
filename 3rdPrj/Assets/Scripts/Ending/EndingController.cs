using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class EndingController : MonoBehaviour
{
    [Header("���丮 �ؽ�Ʈ ���")]
    [TextArea(3, 6)]
    [SerializeField] private List<string> messages;

    [Header("UI ����")]
    [SerializeField] private Text messageText;

    [Header("Ÿ�ڱ� ȿ�� �ӵ�")]
    [SerializeField, Min(0f)] private float typeDelay = 0.05f;

    [Header("���̵� ����")]
    [SerializeField, Min(0.1f)] private float fadeDuration = 0.5f;

    [SerializeField] private string nextSceneName = "LobbyScene";

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typeCoroutine;

    private void Start()
    {
        Time.timeScale = 1f;
        // �� ���� �� ���̵��� �� ������ ����
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
                // Ÿ���� �߿� ��� ��ü ���
                StopCoroutine(typeCoroutine);
                messageText.text = messages[index];
                isTyping = false;
            }
            else
            {
                // �� ��µ� �Ŀ� ���� �޽�����
                if (index < messages.Count - 1)
                    StartCoroutine(NextMessage());
                else
                    StartCoroutine(EndAndLoad());
            }
        }
    }

    private IEnumerator NextMessage()
    {
        // ª�� ���̵�ƿ� ������ (����)
        yield return new WaitForSeconds(fadeDuration);

        index++;
        messageText.text = "";
        typeCoroutine = StartCoroutine(TypeText(messages[index]));
    }

    private IEnumerator EndAndLoad()
    {
        // ��� �޽��� �� �� �� ���̵�ƿ�
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
