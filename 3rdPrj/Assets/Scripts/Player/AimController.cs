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
        // ī�޶� ��ġ�� �������� Ray ����
        Vector3 rayOrigin = _cameraTransform.position;
        Vector3 rayDirection = _cameraTransform.forward;

        // ����ĳ��Ʈ �߻�
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, defaultDistance, aimLayerMask))
        {
            // ������ ��򰡿� �ε��� ������ ������
            AimPoint = hit.point;
        }
        else
        {
            AimPoint = rayOrigin + rayDirection * defaultDistance;
        }


    }

}
