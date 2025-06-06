using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    private Animator animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Move(); 
    }

        public void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetFloat("UltimoX", horizontalInput);
            animator.SetFloat("UltimoY", verticalInput);
        }

        // Actualiza los parámetros para el Animator
        animator.SetFloat("Horizontal", horizontalInput);
        animator.SetFloat("Vertical", verticalInput);

        Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;
        transform.Translate(direction * _speed * Time.deltaTime);
    }
}
