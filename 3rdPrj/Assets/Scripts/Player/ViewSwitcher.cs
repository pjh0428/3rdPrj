using UnityEngine;
using Unity.Cinemachine;

public class ViewSwitcher : MonoBehaviour
{
    [Header("Camera Settings")]
    public CinemachineCamera tpsCamera;
    public CinemachineCamera fpsCamera;
    public KeyCode switchKey = KeyCode.V;

    private bool isFpsView = false;
    private PlayerController _playerController; // PlayerController ��� ���� �߰�

    void Start()
    {
        // PlayerController ������Ʈ ��������
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            isFpsView = !isFpsView;

            // PlayerController�� ���� ���� ��� �˷��ֱ�
            //if (_playerController != null)
            //{
            //    _playerController.SetViewMode(isFpsView);
            //}

            if (isFpsView)
            {
                // FPS ���� ��ȯ
                fpsCamera.Priority = 11;
            }
            else
            {
                // TPS ���� ��ȯ
                tpsCamera.Priority = 10;
                fpsCamera.Priority = 9;
            }
        }
    }
}