using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour, INPCBehavior, IEnemyProximityResponse
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _detectionRadius = 3f;
    public float detectionRadius => _detectionRadius;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float damage = 10f;

    private bool _isFollowing = false;
    private AllyState _state = AllyState.Following;

    private Transform _currentTarget;
    private float _lastAttackTime;
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        // No hace falta asignar velocidad ni stoppingDistance aquí
    }

    private void Update()
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

        if (TryGetComponent(out HandleIndicator indicator)) indicator.DisableIndicator();
        if (TryGetComponent(out PlayerProximityDetector detector)) detector.enabled = false;
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
        Transform enemy = FindNearestReachableEnemy();
        if (enemy != null)
        {
            _currentTarget = enemy;
            _state = AllyState.Attacking;
            Debug.Log($"{gameObject.name} va a atacar a {enemy.name}");
        }
        else
        {
            Debug.Log($"{gameObject.name} no encontró enemigos accesibles para atacar.");
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
        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance > _agent.stoppingDistance)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_player.position);
        }
        else
        {
            _agent.isStopped = true;
        }
    }

    private void AttackEnemies()
    {
        if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
        {
            ReturnToFollowMode();
            return;
        }

        float distance = Vector3.Distance(transform.position, _currentTarget.position);
        if (distance > attackRange)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_currentTarget.position);
        }
        else
        {
            _agent.isStopped = true;
            if (Time.time - _lastAttackTime >= attackCooldown)
            {
                IDamageable target = _currentTarget.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    _lastAttackTime = Time.time;
                }
            }
        }
    }

    private Transform FindNearestReachableEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _detectionLayer);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 enemyPos = hit.transform.position;
                NavMeshPath path = new NavMeshPath();

                if (NavMesh.CalculatePath(transform.position, enemyPos, NavMesh.AllAreas, path) &&
                    path.status == NavMeshPathStatus.PathComplete)
                {
                    float distance = Vector3.Distance(transform.position, enemyPos);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = hit.transform;
                    }
                }
            }
        }

        return nearestEnemy;
    }

    public void OnEnemyEnter(Transform enemy)
    {
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

    public bool CanSeePlayer()
    {
        float dist = Vector3.Distance(transform.position, _player.position);
        if (dist > _detectionRadius) return false;

        NavMeshPath path = new NavMeshPath();
        return NavMesh.CalculatePath(transform.position, _player.position, NavMesh.AllAreas, path) &&
               path.status == NavMeshPathStatus.PathComplete;
    }
}