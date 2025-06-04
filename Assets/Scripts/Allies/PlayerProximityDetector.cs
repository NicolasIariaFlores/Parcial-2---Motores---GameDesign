using UnityEngine;
using UnityEngine.AI;

public class PlayerProximityDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    private IProximityResponse _proximityResponse;
    private FollowPlayer _followPlayer;
    private bool _playerInRange = false;
    private Transform _player;

    private void Start()
    {
        _proximityResponse = GetComponent<IProximityResponse>();
        _followPlayer = GetComponent<FollowPlayer>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        float detectionRadius = _followPlayer.detectionRadius;

        if (_player == null)
            return;

        float distance = Vector3.Distance(transform.position, _player.position);

        // Primero: ¿está dentro del radio?
        if (distance <= detectionRadius)
        {
            // Segundo: ¿tiene camino navegable?
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, _player.position, NavMesh.AllAreas, path)
                           && path.status == NavMeshPathStatus.PathComplete;

            if (hasPath && !_playerInRange)
            {
                _playerInRange = true;
                _proximityResponse?.OnPlayerEnter();
            }
            else if (!hasPath && _playerInRange)
            {
                _playerInRange = false;
                _proximityResponse?.OnPlayerExit();
            }
        }
        else if (_playerInRange)
        {
            _playerInRange = false;
            _proximityResponse?.OnPlayerExit();
        }
    }
}