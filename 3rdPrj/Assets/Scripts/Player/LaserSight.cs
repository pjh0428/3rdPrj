using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserSight : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private AimController _aimController;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _aimController = GetComponentInParent<AimController>();
    }

   
    void LateUpdate()
    {
        if (_aimController != null && _lineRenderer != null)
        {
            // �������� �������� �ѱ� ��ġ�� ����
            _lineRenderer.SetPosition(0, transform.parent.position);

            // �������� ������ AimController�� ����� ���������� ����
            _lineRenderer.SetPosition(1, _aimController.AimPoint);
        }
    }
}