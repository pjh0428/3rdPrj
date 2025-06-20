//using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("ĳ������ �̵� �ӵ�")]
    public float moveSpeed = 5.0f;

    [Tooltip("���콺�� ���� ĳ���Ͱ� ȸ���ϴ� �ӵ�")]
    public float rotationSpeed = 20.0f; // �������� ���� �� ���� �ӵ��� �ָ� ������ �����ϴ�.

    [Tooltip("�߷� ��")]
    public float gravity = -20.0f;


    [Header("Animator Parameters")]
    [Tooltip("�ִϸ��̼� ������ �ε巯�� ����")]
    public float animationDampTime = 0.1f;

    // --- ����� ���� ---
    private CharacterController _characterController;
    private Animator _animator;
    private Transform _cameraTransform;

    private Vector3 _playerVelocity;

    private bool isFpsMode = false; // ���� ��� ���� ����

    // �ܺ�(ViewSwitcher)���� ȣ���� �Լ�
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
            Debug.LogError("���� ī�޶� ã�� �� �����ϴ�! 'MainCamera' �±װ� �����Ǿ����� Ȯ�����ּ���.");
        }

        // ���콺 Ŀ���� ��װ� ������ �ʰ� ����ϴ�. (TPS/FPS ������ �Ϲ����� ����)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. �÷��̾� �Է� �ޱ�
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 2. ���� ���� �̵� ���� ���
        Vector3 moveDirection = CalculateMoveDirection(horizontalInput, verticalInput);

        // 3. ĳ���� �̵�
        _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // 4. ���콺 �������� ĳ���� ȸ�� (���ο� ���� ȣ��)
        RotateCharacterWithMouse();

        // 5. �߷� ó��
        ApplyGravity();

        // 6. �ִϸ����� ������Ʈ
        UpdateAnimator(moveDirection);
    }

    private Vector3 CalculateMoveDirection(float hInput, float vInput)
    {
        // �̵� ���� ��� ������ ������ �ʿ䰡 �����ϴ�.
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

        // FPS ��忡���� ��� ȸ��, TPS ��忡���� �ε巴�� ȸ��
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
        // �ִϸ����� ������ ������ �ʿ䰡 �����ϴ�.
        // ĳ������ ȸ���� �������, ���� ���� �̵� ������ �������� �ִϸ��̼��� �����ϹǷ�
        // �ڷ� �ȱ�, ������ �ȱ�(��Ʈ������)�� ��Ȯ�ϰ� ǥ���˴ϴ�.
        Vector3 localMove = transform.InverseTransformDirection(moveDirection);
        float moveX = localMove.x;
        float moveY = localMove.z;

        _animator.SetFloat("MoveX", moveX, animationDampTime, Time.deltaTime);
        _animator.SetFloat("MoveY", moveY, animationDampTime, Time.deltaTime);
        _animator.SetFloat("MoveSpeed", moveDirection.magnitude);
    }
}