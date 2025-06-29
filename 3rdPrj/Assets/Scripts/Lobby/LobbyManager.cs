using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    [Header("다음 씬 이름")]
    [SerializeField] private string nextSceneName = "IntroScene";

    void Update()
    {
        // Enter 키 누르면 인트로 씬으로 이동
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ScreenFader.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
    }
}
