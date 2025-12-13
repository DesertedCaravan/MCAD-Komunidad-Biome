using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Classes")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private InputActionReference _moveAction;

    [Header("Movement Variables")]
    [SerializeField] private float _walkSpeed; // 5.0f;
    [SerializeField] private float _runSpeed; // 10.0f;

    [Header("Looking Variables")]
    [SerializeField] private float _mouseSensitivity; // 10.0f;
    private Vector2 _mouseDelta;
    // [SerializeField] private float minAngle; // = -89f; // Minimum turn angle
    // [SerializeField] private float maxAngle; // = 89f; // Maximum turn angle
    // float xRotation = 0;

    // [SerializeField] private float zoomFOV; // 30.0f;
    // private bool isZoomed = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        Vector2 moveInput = _moveAction.action.ReadValue<Vector2>();
        float horizontal = moveInput.x; // Equivalent to GetAxis("Horizontal")
        float vertical = moveInput.y; // Equivalent to GetAxis("Horizontal")

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        if (Keyboard.current[Key.LeftShift].wasPressedThisFrame)
        {
            _characterController.Move(move * _walkSpeed * Time.deltaTime);
        }
        else
        {
            _characterController.Move(move * _runSpeed * Time.deltaTime);
        }

        // Look Around
        if (Mouse.current != null)
        {
            _mouseDelta = Mouse.current.delta.ReadValue();
            float mouseX = _mouseDelta.x * _mouseSensitivity * Time.deltaTime;
            // float mouseY = _mouseDelta.y * _mouseSensitivity * Time.deltaTime;

            // Horizontal rotation (player yaw)
            transform.Rotate(Vector3.up * mouseX);

            // Vertical rotation (camera pitch)
            // xRotation -= mouseY;
            // xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);
            // _playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // Zoom
        /*
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isZoomed = !isZoomed;
            _playerCamera.fieldOfView = isZoomed ? zoomFOV : 60.0f;
        }
        */
    }
}