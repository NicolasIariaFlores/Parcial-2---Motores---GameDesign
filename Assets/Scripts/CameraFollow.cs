
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField, Tooltip("Valores entre 0 y 1")]
    private float smooth = 0;

    private Vector3 offset = new Vector3(0, 0, -10);
    private Vector3 vel = Vector3.zero; 

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 position = target.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, position, ref vel, smooth);
            transform.position = smoothedPosition;
        }
    }
}
