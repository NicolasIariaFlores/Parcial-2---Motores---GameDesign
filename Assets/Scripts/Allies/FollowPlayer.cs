using UnityEngine;

public class FollowPlayer : MonoBehaviour, INPCBehavior, IEnemyProximityResponse
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _stopDistance = 1.5f;
    [SerializeField] private float _detectionRadius = 3f;
    public float detectionRadius => _detectionRadius;
    [SerializeField] private int _rayCount = 36;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private float _smoothTime = 0.14f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float damage = 10f;

    private bool _isFollowing = false;
    private AllyState _state = AllyState.Following;

    private Transform _currentTarget;
    private float _lastAttackTime;
    private Rigidbody2D _rb;
    private NPCStats _npcStats;

    private void Start()
    {
        _npcStats = GetComponent<NPCStats>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (!_isFollowing) return;

        switch (_state)
        {
            case AllyState.Following:
                Follow();
                break;
            case AllyState.Attacking:
                AttackEnemies();
                break;
        }
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
    public void UpdateBehavior()
    {
        if (_isFollowing && Input.GetKeyDown(KeyCode.T) && _state != AllyState.Attacking)
        {
            SetToAttackMode();
        }
    }
    private void SetToAttackMode()
    {
        // Solo entra si puede ver un enemigo dentro del rango
        Transform enemy = FindNearestEnemy();
        if (enemy != null)
        {
            _currentTarget = enemy;
            _state = AllyState.Attacking;
            Debug.Log($"{gameObject.name} va a atacar a {enemy.name}");
        }
        else
        {
            Debug.Log($"{gameObject.name} no encontró enemigos dentro del rango de visión.");
        }
    }
    public void SendToAttack()
    {
        if (_isFollowing && _state != AllyState.Attacking)
        {
            SetToAttackMode();
        }
    }
    private void ReturnToFollowMode()
    {
        _state = AllyState.Following;
        _currentTarget = null;
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
    private void AttackEnemies()
    {
        if (_currentTarget == null)
        {
            ReturnToFollowMode();
            return;
        }

        float distance = Vector2.Distance(transform.position, _currentTarget.position);

        if (distance > attackRange)
        {
            Vector2 dir = (_currentTarget.position - transform.position).normalized;
            Vector2 desiredVelocity = dir * _npcStats.BaseSpeed;
            Vector2 smoothVelocity = Vector2.Lerp(_rb.velocity, desiredVelocity, _smoothTime);
            _rb.MovePosition(_rb.position + smoothVelocity * Time.fixedDeltaTime);
        }
        else
        {
            if (Time.time - _lastAttackTime >= attackCooldown)
            {
                IDamageable target = _currentTarget.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    _lastAttackTime = Time.time;
                }
            }

            // Si el enemigo murió
            if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
            {
                ReturnToFollowMode();
            }
        }
    }
    private Transform FindNearestEnemy()
    {
        Vector2 origin = transform.position;
        float angleStep = 360f / _rayCount;
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < _rayCount; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, _detectionRadius, _detectionLayer);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(origin, hit.collider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = hit.collider.transform;
                    Debug.Log($"{gameObject.name} encontró enemigo {hit.collider.name} a distancia {distance}");
                }
            }
        }
        if (nearestEnemy == null)
            Debug.Log($"{gameObject.name} no encontró enemigos.");
        return nearestEnemy;
    }
    public void OnEnemyEnter(Transform enemy)
    {
    // Solo almacena el enemigo más cercano (opcional)
    Debug.Log($"{gameObject.name} detectó a {enemy.name}, pero esperará la orden para atacar.");
    }

    public void OnEnemyExit()
    {
        if (_state == AllyState.Attacking)
        {
            ReturnToFollowMode();
            Debug.Log($"{gameObject.name} dejó de detectar al enemigo.");
        }
    }
}