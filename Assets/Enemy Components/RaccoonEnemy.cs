using UnityEngine;
using UnityEngine.AI;

public class RaccoonEnemy : MonoBehaviour
{
    public TreeObjective tree;
    public float moveSpeed = 3f;
    public float attackDistance = 1.5f;
    public float attackDelay = 1f;
    public float playerFightDistance = 2f;
    public float riseDistance = 1.5f;
    public float riseSpeed = 2f;

    private Enemy enemy;
    private PlayerController player;
    private NavMeshAgent agent;
    private Vector3 surfacePosition;
    private float attackTimer;
    private float slowMultiplier = 1f;
    private float slowTimer;
    private bool isRising = true;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindAnyObjectByType<PlayerController>();

        if (tree == null)
            tree = FindAnyObjectByType<TreeObjective>();

        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
            agent = gameObject.AddComponent<NavMeshAgent>();

        ConfigureAgent();
        agent.enabled = false;
        surfacePosition = transform.position;
        transform.position -= Vector3.up * riseDistance;

        Renderer enemyRenderer = GetComponent<Renderer>();

        if (enemyRenderer != null)
            enemyRenderer.material.color = Color.gray;
    }

    void Update()
    {
        if (tree == null || player == null || !tree.IsAlive)
            return;

        if (isRising)
        {
            transform.position = Vector3.MoveTowards(transform.position, surfacePosition, riseSpeed * Time.deltaTime);

            if (transform.position == surfacePosition)
            {
                isRising = false;
                EnableAgent();
            }

            return;
        }

        if (!agent.isOnNavMesh)
            return;

        attackTimer -= Time.deltaTime;
        slowTimer -= Time.deltaTime;

        if (slowTimer <= 0f)
            slowMultiplier = 1f;

        agent.speed = moveSpeed * slowMultiplier;

        Vector3 treePosition = FlatPosition(tree.transform.position);
        Vector3 playerPosition = FlatPosition(player.transform.position);
        Vector3 treeDirection = (treePosition - transform.position).normalized;
        Vector3 playerDirection = (playerPosition - transform.position).normalized;
        float playerDistance = Vector3.Distance(transform.position, playerPosition);
        bool playerIsBlocking = playerDistance <= playerFightDistance && Vector3.Dot(treeDirection, playerDirection) > 0.5f;
        Vector3 targetPosition = playerIsBlocking ? playerPosition : treePosition;
        float targetDistance = Vector3.Distance(transform.position, targetPosition);

        if (targetDistance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPosition);
        }
        else
        {
            agent.isStopped = true;

            if (attackTimer <= 0f)
            {
                if (playerIsBlocking)
                    player.TakeDamage(enemy.damage);
                else
                    tree.TakeDamage(enemy.damage);

                attackTimer = attackDelay;
            }
        }
    }

    public void SetSlowMultiplier(float amount)
    {
        slowMultiplier = amount;
        slowTimer = 0.2f;
    }

    void EnableAgent()
    {
        agent.enabled = true;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(transform.position, out hit, 3f, NavMesh.AllAreas))
            agent.Warp(hit.position);
    }

    void ConfigureAgent()
    {
        CapsuleCollider enemyCollider = GetComponent<CapsuleCollider>();
        float enemyHeight = enemyCollider.height * transform.localScale.y;
        agent.height = enemyHeight;
        agent.radius = enemyCollider.radius * transform.localScale.x;
        agent.baseOffset = -enemyHeight * 0.5f;
        agent.stoppingDistance = attackDistance;
    }

    Vector3 FlatPosition(Vector3 position)
    {
        position.y = transform.position.y;
        return position;
    }
}
