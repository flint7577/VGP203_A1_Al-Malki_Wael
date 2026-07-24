using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform interactPoint;

    public InputActionReference moveAction;
    public InputActionReference interactAction;
    public InputActionReference throwAction;


    public float moveSpeed = 5f;
    public float interactRange = 2f;
    public float throwForce = 10f;

    private Rigidbody rb;
    private Rigidbody heldObject;
    private Collider heldCollider;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        interactAction.action.Enable();

        if (throwAction != null)
            throwAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        interactAction.action.Disable();

        if (throwAction != null)
            throwAction.action.Disable();
    }
    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();

        if (interactAction.action.WasPressedThisFrame())
        {
            if (heldObject == null)
                Pickup();
            else
                Drop();
        }

        if (throwAction != null && throwAction.action.WasPressedThisFrame() && heldObject != null)
            Throw();
    }

    void FixedUpdate()
    {
        Quaternion playerRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        rb.MoveRotation(playerRotation);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;
        move.Normalize();

        Vector3 velocity = move * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        if (heldObject != null)
        {
            heldObject.MovePosition(interactPoint.position);
            heldObject.MoveRotation(interactPoint.rotation);
        }
    }

    void Pickup()
    {
        if (Physics.Raycast(interactPoint.position, cameraTransform.forward, out RaycastHit hit, interactRange))
        {
            Rigidbody objectRb = hit.collider.attachedRigidbody;

            if (objectRb != null && objectRb != rb)
            {
                heldObject = objectRb;
                heldCollider = hit.collider;

                if (!heldObject.isKinematic)
                {
                    heldObject.linearVelocity = Vector3.zero;
                    heldObject.angularVelocity = Vector3.zero;
                }


                heldCollider.enabled = false;
                heldObject.isKinematic = true;
                heldObject.useGravity = false;
                heldObject.position = interactPoint.position;
                heldObject.rotation = interactPoint.rotation;
            }
        }
    }

    void Drop()
    {
        heldObject.isKinematic = false;
        heldObject.useGravity = true;
        heldCollider.enabled = true;

        heldObject = null;
        heldCollider = null;
    }

    void Throw()
    {
        Rigidbody thrownObject = heldObject;
        AcornProjectile acornProjectile = thrownObject.GetComponent<AcornProjectile>();

        Drop();

        if (acornProjectile != null)
            acornProjectile.Launch(cameraTransform.forward);
        else
            thrownObject.AddForce(cameraTransform.forward * throwForce, ForceMode.Impulse);
    }
}
