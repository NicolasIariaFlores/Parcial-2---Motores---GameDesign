using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _movement; 
    [SerializeField] private float _speed;
    private Vector3 originalScale;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }
    void Update()
    {
        Move(); 
    }

    public void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;

        transform.Translate(direction * _speed * Time.deltaTime);
    }
}
