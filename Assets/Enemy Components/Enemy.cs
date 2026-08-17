using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int ActiveEnemyCount { get; private set; }

    public float maxHealth = 50f;
    public float damage = 10f;

    private float currentHealth;
    private bool counted;

    public float HealthPercent => currentHealth / maxHealth;

    void OnEnable()
    {
        if (!counted)
        {
            ActiveEnemyCount++;
            counted = true;
        }
    }

    void OnDisable()
    {
        if (counted)
        {
            ActiveEnemyCount--;
            counted = false;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        BearEnemy bearEnemy = GetComponent<BearEnemy>();

        if (bearEnemy != null)
            bearEnemy.Aggro();

        if (currentHealth <= 0f)
            Destroy(gameObject);
    }
}
