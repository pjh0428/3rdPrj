using UnityEngine;
using System.Collections;

public class UIShake : MonoBehaviour
{
    public static UIShake Instance { get; private set; }

    [Tooltip("��� ��� RectTransform (Canvas�� Ư�� �г�)")]
    [SerializeField] private RectTransform uiRoot;

    [Header("�⺻ ����")]
    [SerializeField, Min(0f)] private float defaultDuration = 0.2f;
    [SerializeField, Min(0f)] private float defaultMagnitude = 10f;

    private Vector2 originalPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (uiRoot == null) uiRoot = GetComponent<RectTransform>();
        originalPos = uiRoot.anchoredPosition;
    }

    /// <summary>
    /// UI�� ���ϴ�.
    /// duration: ���ӽð�(��), magnitude: ��鸲 ����
    /// </summary>
    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(
            duration > 0f ? duration : defaultDuration,
            magnitude > 0f ? magnitude : defaultMagnitude
        ));
    }

    private IEnumerator ShakeCoroutine(float dur, float mag)
    {
        float elapsed = 0f;
        while (elapsed < dur)
        {
            // ȭ�� ��ǥ(�ȼ�) ������ ���� ������
            float x = Random.Range(-1f, 1f) * mag;
            float y = Random.Range(-1f, 1f) * mag;
            uiRoot.anchoredPosition = originalPos + new Vector2(x, y);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        // ����ġ ����
        uiRoot.anchoredPosition = originalPos;
    }
}
