using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    [Header("다음 씬 이름")]
    [SerializeField] private string nextSceneName = "IntroScene";

    private void Start()
    {
        // 게임 시작 시, 시간 스케일이 0으로 남아있다면 리셋
        Time.timeScale = 1f;
        // 화면 전체 검정에서 밝아지는 페이드 인
        ScreenFader.Instance.FadeIn(onComplete: null, customDuration: 3f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Enter 누르면 검정으로 페이드 아웃 후 씬 전환
            ScreenFader.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
    }
}
