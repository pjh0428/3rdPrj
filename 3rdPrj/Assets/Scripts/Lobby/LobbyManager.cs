using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    [Header("���� �� �̸�")]
    [SerializeField] private string nextSceneName = "IntroScene";

    private void Start()
    {
        // ���� ���� ��, �ð� �������� 0���� �����ִٸ� ����
        Time.timeScale = 1f;
        // ȭ�� ��ü �������� ������� ���̵� ��
        ScreenFader.Instance.FadeIn(onComplete: null, customDuration: 3f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Enter ������ �������� ���̵� �ƿ� �� �� ��ȯ
            ScreenFader.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
    }
}
