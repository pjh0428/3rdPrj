using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HighlightRing : MonoBehaviour
{
    [Header("Ring Settings")]
    [SerializeField, Range(16, 128)] private int segments = 64;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private Color color = Color.yellow;

    [Header("Pulse Colors (RGBA 0~255)")]
    [SerializeField] private Color32 startColor = new Color32(225, 225, 0, 225);
    [SerializeField] private Color32 endColor = new Color32(255, 255, 0, 35);

    [Header("Pulse Settings")]
    [SerializeField, Min(0f)] private float pulseSpeed = 2f;  // 속도

    private LineRenderer lr;
    private Color32[] colors;

    private void Awake()
    {
        // LineRenderer 컴포넌트 가져오기
        lr = GetComponent<LineRenderer>();

        // 머티리얼 & 컬러 설정 (Unlit/Color 계열)
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = lr.endWidth = lineWidth;
        lr.loop = true;
        lr.useWorldSpace = false;
        BuildRing();
    }

    private void Update()
    {
        // t: 0↔1을 펄스 속도로 왕복
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        Color32 c = Color32.Lerp(startColor, endColor, t);
        lr.startColor = lr.endColor = c;
    }

    private void BuildRing()
    {
        lr.positionCount = segments;
        float delta = 2 * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float ang = i * delta;
            float x = Mathf.Cos(ang) * radius;
            float z = Mathf.Sin(ang) * radius;
            lr.SetPosition(i, new Vector3(x, 0f, z));
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (!lr) lr = GetComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = lineWidth;
        BuildRing();
    }
    #endif
}
