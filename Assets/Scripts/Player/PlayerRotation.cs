
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Rigidbody2D rb;
    private float angle; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }


    private void FixedUpdate()
    {
        LookAt();
    }

    //TODO: Giro mas smooth??? 
    void LookAt()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        Vector2 direction = mousePos - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);
    }
}
