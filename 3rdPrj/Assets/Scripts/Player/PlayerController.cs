using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4.0f;
    public float gravity = -20.0f;

    [Header("Mouse Look Settings")]
    [Tooltip("���콺 ����")]
    public float mouseSensitivity = 2.0f;
    [Tooltip("ī�޶��� ���� ȸ���� ����� ��Ŀ ������Ʈ (FP_Camera_Anchor)")]
    public Transform cameraAnchor;
    [Tooltip("ī�޶� ���� �þ߰� ���� (�Ʒ�)")]
    public float verticalLookMin = -40.0f;
    [Tooltip("ī�޶� ���� �þ߰� ���� (��)")]
    public float verticalLookMax = 40.0f;

    [Header("Animator Parameters")]
    public float animationDampTime = 0.1f;

    
    private CharacterController _characterController;
    private AimController _aimController;
    private Animator _animator;
    private Vector3 _playerVelocity;
    private float _cameraPitch = 0.0f; // ī�޶��� ���� ȸ�� ���� ����

    private bool _isInputEnabled = false; // �÷��̾� �Է� Ȱ��ȭ ����

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
        // ���콺�� �¿� ������ ���� �����ɴϴ�.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        // ���콺�� ���� ������ ���� �����ɴϴ�.
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
      
        transform.Rotate(Vector3.up * mouseX);

        _cameraPitch -= mouseY;
        _cameraPitch = Mathf.Clamp(_cameraPitch, verticalLookMin, verticalLookMax); // ���� ���� ����
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
        // ������ �����̵��� ���
        yield return new WaitForSeconds(delay);

        // �Է� Ȱ��ȭ
        _isInputEnabled = true;
    }

}