using System;
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

    //AUDIO
    [SerializeField] private AudioClip[] idleClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private float idleSoundMin;
    [SerializeField] private float idleSoundMax;

    private float _timeToNextIdleAudioSound;
    private AudioSource _audioSource;

    //referencias de otros codigos
    private bool _isFollowing = false;
    private AllyState _state = AllyState.Idle;
    private NPCStats _stats;

    private Transform _currentTarget;
    private float _lastAttackTime;
    private NavMeshAgent _agent;
    private float _defaultSpeed;

    //animator jiji
    private Animator _animator;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _defaultSpeed = _agent.speed;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _animator = GetComponent<Animator>();
        _stats = GetComponent<NPCStats>();
        _audioSource = GetComponent<AudioSource>();
        _timeToNextIdleAudioSound = Time.time + UnityEngine.Random.Range(idleSoundMin, idleSoundMax);
    }

    private void Update()
    {
        // if (!_isFollowing) return;

        switch (_state)
        {
            case AllyState.Idle:
                IdleSoundHandler();
                break;
            case AllyState.Following:
                if (_isFollowing)
                    Follow();
                break;
            case AllyState.Attacking:
                if (_isFollowing)
                    AttackEnemies();
                break;
        }
    }

    private void IdleSoundHandler()
    {
        if (Time.time >= _timeToNextIdleAudioSound && idleClips.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, idleClips.Length);
            _audioSource.PlayOneShot(idleClips[index]);

            float delay = UnityEngine.Random.Range(idleSoundMin, idleSoundMax);
            _timeToNextIdleAudioSound = Time.time + delay;
        }
    }

    public void Interact()
    {
        _isFollowing = true;
        _state = AllyState.Following;
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
    private void UpdateAnimations(Vector3 direction)
    {
        _animator.SetFloat("MovimientoX", direction.x);
        _animator.SetFloat("MovimientoY", direction.y);

        if (direction.magnitude > 0.01f)
        {
            _animator.SetFloat("UltimoX", direction.x);
            _animator.SetFloat("UltimoY", direction.y);
        }
    }


    private void Follow()
    {
        _agent.speed = _defaultSpeed;
        _agent.stoppingDistance = 3f;
        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance > _agent.stoppingDistance)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_player.position);

            Vector3 direction = (_player.position - transform.position).normalized;
            UpdateAnimations(direction);
        }
        else
        {
            _agent.isStopped = true;
            UpdateAnimations(Vector3.zero);
        }
        UpdateAnimations(_agent.velocity.normalized);
    }

    private void AttackEnemies()
    {
        _agent.speed = 3f;
        _agent.stoppingDistance = 0.5f;

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

            Vector3 direction = (_currentTarget.position - transform.position).normalized;
            UpdateAnimations(direction);
        }
        else
        {
            _agent.isStopped = true;
            UpdateAnimations(Vector3.zero);

            if (Time.time - _lastAttackTime >= attackCooldown)
            {
                IDamageable target = _currentTarget.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(_stats.Damage);
                    int index = UnityEngine.Random.Range(0, attackClips.Length);
                    _audioSource.PlayOneShot(attackClips[index]);
                    _lastAttackTime = Time.time;
                    //_animator.SetTrigger("Atacar");
                }
            }
        }

        UpdateAnimations(_agent.velocity.normalized);
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
    public void SetPlayer(Transform player)
{
    _player = player;
}
}