using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class LobbyVideoSetup : MonoBehaviour
{
    [Header("������ UI RawImage")]
    [SerializeField] private RawImage videoBG;

    [Header("����� Ŭ��")]
    [SerializeField] private VideoClip videoClip;

    [Header("Audio (�ʿ��ϸ�)")]
    [SerializeField] private AudioSource audioSource;

    private VideoPlayer vp;
    private RenderTexture rt;

    void Awake()
    {
        vp = GetComponent<VideoPlayer>();

        // 1) VideoPlayer ����
        vp.playOnAwake = false;
        vp.isLooping = true;
        vp.clip = videoClip;
        vp.renderMode = VideoRenderMode.RenderTexture;

        // ����� ���� ���
        if (audioSource != null)
        {
            vp.audioOutputMode = VideoAudioOutputMode.AudioSource;
            vp.SetTargetAudioSource(0, audioSource);
        }
        else
        {
            vp.audioOutputMode = VideoAudioOutputMode.None;
        }

        // 2) ��Ÿ������ RenderTexture ����
        int w = Screen.width;
        int h = Screen.height;
        rt = new RenderTexture(w, h, 0);
        rt.Create();

        vp.targetTexture = rt;

        // 3) RawImage�� �ؽ�ó ����
        if (videoBG != null)
            videoBG.texture = rt;
    }

    void Start()
    {
        // �� ���۰� ���ÿ� ���
        vp.Play();
        if (audioSource != null)
            audioSource.Play();
    }

    void OnDestroy()
    {
        // ����
        if (rt != null)
            rt.Release();
    }
}
