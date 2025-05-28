using UnityEngine;

public class PlayerProximityDetector : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private LayerMask _playerLayer;

    private IProximityResponse proximityResponse;
    private bool playerInRange = false;

    private void Start()
    {
        proximityResponse = GetComponent<IProximityResponse>();
    }

    private void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, _detectionRadius, _playerLayer);

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