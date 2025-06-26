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
            // 레이저의 시작점을 총구 위치로 설정
            _lineRenderer.SetPosition(0, transform.parent.position);

            // 레이저의 끝점을 AimController가 계산한 조준점으로 설정
            _lineRenderer.SetPosition(1, _aimController.AimPoint);
        }
    }
}