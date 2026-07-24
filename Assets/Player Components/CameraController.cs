using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    public Transform playerTransform;

    public InputActionReference lookAction;

    public float mouseSensitivity = 15f;
    public float distanceFromPlayer = 5f;
    public float playerHeight = 2f;

    private float mouseX;
    private float mouseY;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void OnEnable()
    {
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        mouseX += lookInput.x * mouseSensitivity * Time.deltaTime;
        mouseY -= lookInput.y * mouseSensitivity * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -20f, 60f);

        Quaternion cameraRotation = Quaternion.Euler(mouseY, mouseX, 0f);
        Vector3 cameraPosition = playerTransform.position - cameraRotation * Vector3.forward * distanceFromPlayer;

        cameraPosition.y += playerHeight;

        transform.position = cameraPosition;
        transform.LookAt(playerTransform.position + Vector3.up);

    }
}
