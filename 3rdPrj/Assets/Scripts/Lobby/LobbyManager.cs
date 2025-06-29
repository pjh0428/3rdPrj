using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    [Header("���� �� �̸�")]
    [SerializeField] private string nextSceneName = "IntroScene";

    void Update()
    {
        // Enter Ű ������ ��Ʈ�� ������ �̵�
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ScreenFader.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
    }
}
