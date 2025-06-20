using UnityEngine;
using Unity.Cinemachine;

public class ViewSwitcher : MonoBehaviour
{
    [Header("Camera Settings")]
    public CinemachineCamera tpsCamera;
    public CinemachineCamera fpsCamera;
    public KeyCode switchKey = KeyCode.V;

    private bool isFpsView = false;
    private PlayerController _playerController; // PlayerController 제어를 위해 추가

    void Start()
    {
        // PlayerController 컴포넌트 가져오기
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            isFpsView = !isFpsView;

            // PlayerController에 현재 시점 모드 알려주기
            if (_playerController != null)
            {
                _playerController.SetViewMode(isFpsView);
            }

            if (isFpsView)
            {
                // FPS 모드로 전환
                fpsCamera.Priority = 11;
            }
            else
            {
                // TPS 모드로 전환
                tpsCamera.Priority = 10;
                fpsCamera.Priority = 9;
            }
        }
    }
}