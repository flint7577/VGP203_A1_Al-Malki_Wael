using UnityEngine;

public class AcornProjectile : MonoBehaviour
{

    public float speed = 10f;  

    private Rigidbody rb;
    private Vector3 direction;
    private bool isMoving;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 launchDirection)
    {
        direction = launchDirection.normalized;
        rb.isKinematic = true; // Disable physics simulation
        rb.useGravity = false; // Disable gravity
        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
           Vector3 nextPosition = rb.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(nextPosition);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isMoving)
        {
            isMoving = false;
            rb.isKinematic = false; // Re-enable physics simulation
            rb.useGravity = true; // Re-enable gravity
        }
    }
}
