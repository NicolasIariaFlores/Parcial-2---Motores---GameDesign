using UnityEngine;

public class FollowPlayer : MonoBehaviour, INPCBehavior
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _stopDistance = 1.5f;
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private int _rayCount = 36;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private float _smoothTime = 0.14f;

    private bool _isFollowing = false;
    private Rigidbody2D _rb;
    private NPCStats _npcStats;

    private void Start()
    {
        _npcStats = GetComponent<NPCStats>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (_isFollowing)
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
        _isFollowing = true;
        Debug.Log($"{gameObject.name} ahora está siguiendo al jugador.");

        // Apagar el indicador si existe
        HandleIndicator indicator = GetComponent<HandleIndicator>();
        if (indicator != null)
        {
            indicator.DisableIndicator();
        }
        PlayerProximityDetector detector = GetComponent<PlayerProximityDetector>();
        if (detector != null)
        {
            detector.enabled = false;
        }
    }

    private void Follow()
    {
        float distance = Vector2.Distance(transform.position, _player.position);
        if (distance > _stopDistance)
        {
            Vector2 directionToPlayer = (_player.position - transform.position).normalized;
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
            float speed = _npcStats.BaseSpeed;

            // Movimiento
            Vector2 desiredVelocity = finalDirection * speed;
            Vector2 smoothVelocity = Vector2.Lerp(_rb.velocity, desiredVelocity, _smoothTime);
            _rb.MovePosition(_rb.position + smoothVelocity * Time.fixedDeltaTime);
        }
    }
    public bool CanSeePlayerCircular()
    {
        Vector2 origin = transform.position;
        float angleStep = 360f / _rayCount;

        for (int i = 0; i < _rayCount; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, _detectionRadius, _detectionLayer);
            Debug.DrawRay(origin, dir * _detectionRadius, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}