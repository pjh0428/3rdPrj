using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    [Header("다음 씬 이름")]
    [SerializeField] private string nextSceneName = "IntroScene";
    [SerializeField] private FadePanelsController fadeController;

    void Update()
    {
        // Enter 키 누르면 인트로 씬으로 이동
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
