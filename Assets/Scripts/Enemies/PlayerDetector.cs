using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Transform target;
    [Tooltip("Radio de deteccion al Player")]
    [SerializeField] private float detectionRadio;
    [SerializeField] private float speed; 

    private void Update()
    {
        MoveTo();
    }

    public void MoveTo()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer < detectionRadio)
        {
            Vector2 direction = (target.position - transform.position).normalized;

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
