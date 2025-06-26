using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HighlightRing : MonoBehaviour
{
    [Header("Ring Settings")]
    [SerializeField, Range(16, 128)] private int segments = 64;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private Color color = Color.yellow;

    private LineRenderer lr;

    private void Awake()
    {
        // LineRenderer 컴포넌트 가져오기
        lr = GetComponent<LineRenderer>();

        // 머티리얼 & 컬러 설정 (Unlit/Color 계열)
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = lineWidth;
        lr.loop = true;
        lr.useWorldSpace = false;  // 이 오브젝트 로컬 좌표로 그리기

        BuildRing();
    }

    private void BuildRing()
    {
        lr.positionCount = segments;
        float deltaAngle = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float ang = Mathf.Deg2Rad * (i * deltaAngle);
            // XZ 평면(지면)에 그린다고 가정. 1인 radius 만큼 떨어진 지점.
            float x = Mathf.Cos(ang) * radius;
            float z = Mathf.Sin(ang) * radius;
            lr.SetPosition(i, new Vector3(x, 0f, z));
        }
    }

    // Inspector에서 radius, color 등을 바꾸면 즉시 반영되길 원하면 아래를 추가:
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (lr == null) lr = GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.startColor = lr.endColor = color;
            lr.startWidth = lr.endWidth = lineWidth;
            BuildRing();
        }
    }
#endif
}
