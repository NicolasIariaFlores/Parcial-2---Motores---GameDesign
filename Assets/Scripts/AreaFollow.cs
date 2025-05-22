using UnityEngine;

public class FollowPlayer : MonoBehaviour, INPCBehavior
{
    public Transform player;
    [SerializeField] private float followSpeed = 4f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private int rayCount = 36;
    [SerializeField] private LayerMask detectionLayer;

    private bool isFollowing = false;

    private void Update()
    {
        if (isFollowing)
        {
            Follow();
        }
    }

    public void UpdateBehavior()
    {
        // Esto se llama desde un gestor central
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
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * followSpeed * Time.deltaTime);
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