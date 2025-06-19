
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Transform targetP;
    public Transform targetA;
    [Tooltip("Radio de deteccion al Player")]
    [SerializeField] private float detectionRadio;
    [SerializeField] private float speed;

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (PlayerInRange())
        {
            MoveToPlayer();
        }
        else if (AllyInRange())
        {
            MoveToAlly(); 
        }
    }

    public void FindPlayer()
    {
        if (targetP == null && targetA == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject ally = GameObject.FindGameObjectWithTag("Ally");
            if (player != null && ally != null)
            {
                targetP = player.transform;
                targetA = ally.transform;
            }
        }
    }
    public bool AllyInRange()
    {
        if (targetA == null)
        {
            Debug.LogWarning("No tiene un target asignado");
            return false;
        }
        return Vector2.Distance(transform.position, targetA.position) < detectionRadio;
    }

    public bool PlayerInRange()
    {
        if (targetP == null)
        {
            Debug.LogWarning("No tiene un target asignado");
            return false;
        }
        return Vector2.Distance(transform.position, targetP.position) < detectionRadio;
    }

    public void MoveToPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, targetP.position);

        if (distanceToPlayer < detectionRadio)
        {
            Vector2 direction = (targetP.position - transform.position).normalized;

            transform.position += (Vector3)(direction * speed * Time.deltaTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void MoveToAlly()
    {
        float distanceToAlly = Vector2.Distance(transform.position, targetA.position);

        if (distanceToAlly < detectionRadio)
        {
            Vector2 direction = (targetA.position - transform.position).normalized;

            transform.position += (Vector3)(direction * speed * Time.deltaTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadio);
    }
}
