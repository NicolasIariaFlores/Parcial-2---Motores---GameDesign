using UnityEngine;

public class FollowPlayer : MonoBehaviour, INPCBehavior
{
    public Transform player;
    //Cambie la speed por la speed de los stats del personaje jiji
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private int rayCount = 36;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float smoothTime = 0.15f;

    private bool isFollowing = false;
    private Rigidbody2D rb;
    private NPCStats npcStats;

    private void Start()
    {
        npcStats = GetComponent<NPCStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isFollowing)
        {
            Follow();
        }
    }

    public void UpdateBehavior()
    {
        // c
    }

    public void Interact()
    {
        isFollowing = true;
        Debug.Log($"{gameObject.name} ahora está siguiendo al jugador.");
    }

    private void Follow()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > stopDistance)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            // Detección de obstáculos
            RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, directionToPlayer, 1.5f, LayerMask.GetMask("Obstacles"));
            Debug.DrawRay(transform.position, directionToPlayer * 1.5f, Color.green);

            if (obstacleHit.collider != null)
            {
                directionToPlayer = Vector2.Perpendicular(directionToPlayer).normalized;
            }

            // Separacion entre los Zombies aliados
            Vector2 separation = Vector2.zero;
            Collider2D[] nearbyAllies = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D ally in nearbyAllies)
            {
                if (ally.gameObject != this.gameObject && ally.CompareTag("Ally"))
                {
                    Vector2 away = (Vector2)(transform.position - ally.transform.position);
                    float dist = away.magnitude;
                    if (dist > 0)
                    {
                        separation += away.normalized / dist;
                    }
                }
            }
            // Dirección
            Vector2 finalDirection = (directionToPlayer + separation).normalized;
            float speed = npcStats.BaseSpeed;

            // Movimiento
            Vector2 desiredVelocity = finalDirection * speed;
            Vector2 smoothVelocity = Vector2.Lerp(rb.velocity, desiredVelocity, smoothTime);
            rb.MovePosition(rb.position + smoothVelocity * Time.fixedDeltaTime);
        }
    }
    public bool CanSeePlayerCircular()
    {
        Vector2 origin = transform.position;
        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, detectionRadius, detectionLayer);
            Debug.DrawRay(origin, dir * detectionRadius, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}