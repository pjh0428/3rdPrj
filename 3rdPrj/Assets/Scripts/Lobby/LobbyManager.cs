using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    [Header("���� �� �̸�")]
    [SerializeField] private string nextSceneName = "IntroScene";
    [SerializeField] private FadePanelsController fadeController;

    void Update()
    {
        // Enter Ű ������ ��Ʈ�� ������ �̵�
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fadeController.StartFade();
            StartCoroutine(LoadAfterFade());
        }
    }

    private IEnumerator LoadAfterFade()
    {
        yield return new WaitForSeconds(fadeController.Duration);
        SceneManager.LoadScene(nextSceneName);
    }
}
