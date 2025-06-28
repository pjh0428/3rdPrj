using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class LobbyVideoSetup : MonoBehaviour
{
    [Header("연결할 UI RawImage")]
    [SerializeField] private RawImage videoBG;

    [Header("재생할 클립")]
    [SerializeField] private VideoClip videoClip;

    [Header("Audio (필요하면)")]
    [SerializeField] private AudioSource audioSource;

    private VideoPlayer vp;
    private RenderTexture rt;

    void Awake()
    {
        vp = GetComponent<VideoPlayer>();

        // 1) VideoPlayer 세팅
        vp.playOnAwake = false;
        vp.isLooping = true;
        vp.clip = videoClip;
        vp.renderMode = VideoRenderMode.RenderTexture;

        // 오디오 붙일 경우
        if (audioSource != null)
        {
            vp.audioOutputMode = VideoAudioOutputMode.AudioSource;
            vp.SetTargetAudioSource(0, audioSource);
        }
        else
        {
            vp.audioOutputMode = VideoAudioOutputMode.None;
        }

        // 2) 런타임으로 RenderTexture 생성
        int w = Screen.width;
        int h = Screen.height;
        rt = new RenderTexture(w, h, 0);
        rt.Create();

        vp.targetTexture = rt;

        // 3) RawImage에 텍스처 지정
        if (videoBG != null)
            videoBG.texture = rt;
    }

    void Start()
    {
        // 씬 시작과 동시에 재생
        vp.Play();
        if (audioSource != null)
            audioSource.Play();
    }

    void OnDestroy()
    {
        // 정리
        if (rt != null)
            rt.Release();
    }
}
