using UnityEngine;

public class PlayerProximityDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    private IProximityResponse proximityResponse;
    private bool playerInRange = false;
    private FollowPlayer _detectionRadius;

    private void Start()
    {
        proximityResponse = GetComponent<IProximityResponse>();
        _detectionRadius = GetComponent<FollowPlayer>();
    }

    private void Update()
    {
        float detectionRadius = _detectionRadius.detectionRadius;
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, _playerLayer);

        if (player != null && !playerInRange)
        {
            playerInRange = true;
            proximityResponse?.OnPlayerEnter();
        }
        else if (player == null && playerInRange)
        {
            playerInRange = false;
            proximityResponse?.OnPlayerExit();
        }
    }
}