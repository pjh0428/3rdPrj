using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float gravity = -20.0f;

    [Header("Mouse Look Settings")]
    [Tooltip("마우스 감도")]
    public float mouseSensitivity = 2.0f;
    [Tooltip("카메라의 상하 회전을 담당할 앵커 오브젝트 (FP_Camera_Anchor)")]
    public Transform cameraAnchor;
    [Tooltip("카메라 상하 시야각 제한 (아래)")]
    public float verticalLookMin = -80.0f;
    [Tooltip("카메라 상하 시야각 제한 (위)")]
    public float verticalLookMax = 80.0f;

    [Header("Animator Parameters")]
    public float animationDampTime = 0.1f;

    
    private CharacterController _characterController;
    private AimController _aimController;
    private Animator _animator;
    private Vector3 _playerVelocity;
    private float _cameraPitch = 0.0f; // 카메라의 상하 회전 각도 저장

    private bool _isInputEnabled = false; // 플레이어 입력 활성화 여부

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _aimController = GetComponent<AimController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(EnableInputAfterDelay(0.3f));
    }

    void Update()
    {
        if (!_isInputEnabled) return;

        HandleMouseLook();

        HandleMovement();
    }

    private void HandleMouseLook()
    {
        // 마우스의 좌우 움직임 값을 가져옵니다.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        // 마우스의 상하 움직임 값을 가져옵니다.
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
      
        transform.Rotate(Vector3.up * mouseX);

        _cameraPitch -= mouseY;
        _cameraPitch = Mathf.Clamp(_cameraPitch, verticalLookMin, verticalLookMax); // 상하 각도 제한
        cameraAnchor.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
    }

    private void HandleMovement()
    {
        ApplyGravity();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.forward * verticalInput) + (transform.right * horizontalInput);

        _characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

        _animator.SetFloat("MoveX", horizontalInput, animationDampTime, Time.deltaTime);
        _animator.SetFloat("MoveY", verticalInput, animationDampTime, Time.deltaTime);
        _animator.SetFloat("MoveSpeed", moveDirection.magnitude);
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        _playerVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    IEnumerator EnableInputAfterDelay(float delay)
    {
        // 지정된 딜레이동안 대기
        yield return new WaitForSeconds(delay);

        // 입력 활성화
        _isInputEnabled = true;
    }

}