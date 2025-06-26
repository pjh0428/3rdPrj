using UnityEngine;

public class AimController : MonoBehaviour
{
    [Header("Aim Settings")]
    public LayerMask aimLayerMask;
    public float defaultDistance = 100f;

    public Vector3 AimPoint { get; private set; }

    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // 카메라 위치와 방향으로 Ray 생성
        Vector3 rayOrigin = _cameraTransform.position;
        Vector3 rayDirection = _cameraTransform.forward;

        // 레이캐스트 발사
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, defaultDistance, aimLayerMask))
        {
            // 광선이 어딘가에 부딪힌 지점이 조준점
            AimPoint = hit.point;
        }
        else
        {
            AimPoint = rayOrigin + rayDirection * defaultDistance;
        }


    }

}
