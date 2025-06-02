using UnityEngine;

public class EnemyProximityDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private LayerMask enemyLayer;
    private IEnemyProximityResponse _response;
    private Transform _currentEnemy;

    private void Start()
    {
        _response = GetComponent<IEnemyProximityResponse>();
    }

    private void Update()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, detectionRadius, enemyLayer);

        if (enemy != null && _currentEnemy == null)
        {
            _currentEnemy = enemy.transform;
            _response?.OnEnemyEnter(_currentEnemy);
        }
        else if (enemy == null && _currentEnemy != null)
        {
            _response?.OnEnemyExit();
            _currentEnemy = null;
        }
    }
}