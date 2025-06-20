//using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("캐릭터의 이동 속도")]
    public float moveSpeed = 5.0f;

    [Tooltip("마우스를 따라 캐릭터가 회전하는 속도")]
    public float rotationSpeed = 20.0f; // 이전보다 조금 더 빠른 속도를 주면 느낌이 좋습니다.

    [Tooltip("중력 값")]
    public float gravity = -20.0f;


    [Header("Animator Parameters")]
    [Tooltip("애니메이션 블렌딩의 부드러움 정도")]
    public float animationDampTime = 0.1f;

    // --- 비공개 변수 ---
    private CharacterController _characterController;
    private Animator _animator;
    private Transform _cameraTransform;

    private Vector3 _playerVelocity;

    private bool isFpsMode = false; // 현재 모드 저장 변수

    // 외부(ViewSwitcher)에서 호출할 함수
    public void SetViewMode(bool isFps)
    {
        isFpsMode = isFps;
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("메인 카메라를 찾을 수 없습니다! 'MainCamera' 태그가 지정되었는지 확인해주세요.");
        }

        // 마우스 커서를 잠그고 보이지 않게 만듭니다. (TPS/FPS 게임의 일반적인 설정)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. 플레이어 입력 받기
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 2. 월드 기준 이동 방향 계산
        Vector3 moveDirection = CalculateMoveDirection(horizontalInput, verticalInput);

        // 3. 캐릭터 이동
        _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // 4. 마우스 방향으로 캐릭터 회전 (새로운 로직 호출)
        RotateCharacterWithMouse();

        // 5. 중력 처리
        ApplyGravity();

        // 6. 애니메이터 업데이트
        UpdateAnimator(moveDirection);
    }

    private Vector3 CalculateMoveDirection(float hInput, float vInput)
    {
        // 이동 방향 계산 로직은 변경할 필요가 없습니다.
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        return (cameraForward * vInput) + (cameraRight * hInput);
    }

    private void RotateCharacterWithMouse()
    {
        float targetAngle = _cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        // FPS 모드에서는 즉시 회전, TPS 모드에서는 부드럽게 회전
        if (isFpsMode)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
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

    private void UpdateAnimator(Vector3 moveDirection)
    {
        // 애니메이터 로직도 변경할 필요가 없습니다.
        // 캐릭터의 회전과 관계없이, 실제 월드 이동 방향을 기준으로 애니메이션을 결정하므로
        // 뒤로 걷기, 옆으로 걷기(스트레이핑)가 정확하게 표현됩니다.
        Vector3 localMove = transform.InverseTransformDirection(moveDirection);
        float moveX = localMove.x;
        float moveY = localMove.z;

        _animator.SetFloat("MoveX", moveX, animationDampTime, Time.deltaTime);
        _animator.SetFloat("MoveY", moveY, animationDampTime, Time.deltaTime);
        _animator.SetFloat("MoveSpeed", moveDirection.magnitude);
    }
}