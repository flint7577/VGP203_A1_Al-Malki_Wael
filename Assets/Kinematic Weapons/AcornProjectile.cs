using UnityEngine;

public class AcornProjectile : MonoBehaviour
{

    public float speed = 10f;
    public float damage = 25f;

    private Rigidbody rb;
    private Vector3 direction;
    private bool isMoving;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PreparePickup()
    {
        isMoving = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
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
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (isMoving && enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (isMoving)
        {
            isMoving = false;
            rb.isKinematic = false; // Re-enable physics simulation
            rb.useGravity = true; // Re-enable gravity
        }
    }
}
