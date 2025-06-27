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
        // LineRenderer ������Ʈ ��������
        lr = GetComponent<LineRenderer>();

        // ��Ƽ���� & �÷� ���� (Unlit/Color �迭)
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = lineWidth;
        lr.loop = true;
        lr.useWorldSpace = false;  // �� ������Ʈ ���� ��ǥ�� �׸���

        BuildRing();
    }

    private void BuildRing()
    {
        lr.positionCount = segments;
        float deltaAngle = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float ang = Mathf.Deg2Rad * (i * deltaAngle);
            // XZ ���(����)�� �׸��ٰ� ����. 1�� radius ��ŭ ������ ����.
            float x = Mathf.Cos(ang) * radius;
            float z = Mathf.Sin(ang) * radius;
            lr.SetPosition(i, new Vector3(x, 0f, z));
        }
    }

    // Inspector���� radius, color ���� �ٲٸ� ��� �ݿ��Ǳ� ���ϸ� �Ʒ��� �߰�:
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
