
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Transform targetP;
    public Transform targetA;

    [Tooltip("Radio de deteccion")]
    [SerializeField] private float detectionRadio;
    [SerializeField] private float speed;

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        FindAllies();
        if (PlayerInRange())
        {
            MoveToTarget(targetP);
        }
        else if (AllyInRange())
        {
            MoveToTarget(targetA); 
        }
    }

    public void FindPlayer()
    {
        if (targetP == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                targetP = player.transform;
            }
        }
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

    public void FindAllies()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

        float minDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject ally in allies)
        {
            float distance = Vector2.Distance(transform.position, ally.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = ally.transform;
            }
        }

        targetA = nearest;
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

    private void MoveToTarget(Transform target)
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadio);
    }
}
