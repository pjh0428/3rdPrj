using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class IntroController : MonoBehaviour
{
    [Header("���丮 �ؽ�Ʈ ���")]
    [TextArea(3, 6)]           // �ּ� 3��, �ִ� 6�� ������
    [SerializeField]
    private List<string> messages;
    [Header("�׸�(Sprite) ���")]
    [SerializeField] private List<Sprite> pictures;

    [Header("UI ����")]
    [SerializeField] private Image pictureImage;
    [SerializeField] private Text messageText;

    [Header("���̵� ����")]
    [SerializeField, Min(0.1f)] private float fadeDuration = 0.5f;
    [Header("Ÿ�ڱ� ȿ�� �ӵ�")]
    [SerializeField, Min(0f)] private float typeDelay = 0.05f;

    [Header("�� ��ȯ ���̵� �ð�")]
    [SerializeField, Min(0.1f)] private float endFadeDuration = 1.5f;

    [SerializeField] private string nextSceneName = "LobbyScene";

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typeCoroutine;

    private void Start()
    {
        // �� �ε�� �������� ���� �ִ� ���¿���
        ScreenFader.Instance.FadeIn(() =>
        {
            // FadeIn ���� �ڿ��� �Ʒ� ��Ʈ�� ���� ����
            BeginIntroSequence();
        });
    }
    private void BeginIntroSequence()
    {
        // ���� Start() ���� (�޽��� �ʱ�ȭ/Ÿ���� ��)�� �̰����� �ű⼼��.
        messageText.text = "";
        pictureImage.sprite = pictures[index];
        SetImageAlpha(1f);
        typeCoroutine = StartCoroutine(TypeText(messages[index]));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // Ÿ���� �߿��� ���� ��¸�
                StopCoroutine(typeCoroutine);
                messageText.text = messages[index];
                isTyping = false;
            }
            else
            {
                // ���� ��µ� ���¿����� ��������
                if (index < messages.Count - 1)
                    StartCoroutine(ShowNext());
                else
                    // ������ �޽��� �� ���̵�ƿ� �� �� �ε�
                    ScreenFader.Instance.FadeOut(
                        onComplete: () => SceneManager.LoadScene(nextSceneName),
                        customDuration: endFadeDuration
                    );
            }
        }
    }

    private IEnumerator ShowNext()
    {
        // �׸� ���̵� �ƿ�
        yield return StartCoroutine(FadeImage(1f, 0f));

        // ���� �ε���
        index++;
        pictureImage.sprite = pictures[index];

        // �׸� ���̵� ��
        yield return StartCoroutine(FadeImage(0f, 1f));

        // �޽��� �ʱ�ȭ �� Ÿ����
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
