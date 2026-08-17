using UnityEngine;

public class HoneyPuddle : MonoBehaviour
{
    public float slowMultiplier = 0.5f;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        Collider[] objectsInHoney = Physics.OverlapSphere(transform.position, transform.localScale.x);

        foreach (Collider objectInHoney in objectsInHoney)
        {
            PlayerController player = objectInHoney.GetComponent<PlayerController>();

            if (player != null)
                player.SetSlowMultiplier(slowMultiplier);

            RaccoonEnemy raccoon = objectInHoney.GetComponent<RaccoonEnemy>();

            if (raccoon != null)
                raccoon.SetSlowMultiplier(slowMultiplier);
        }
    }
}
