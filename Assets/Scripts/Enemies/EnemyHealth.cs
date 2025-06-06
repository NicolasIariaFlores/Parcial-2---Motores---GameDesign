
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameObject zombie; 
    private bool isDead = false;
    private EnemyPatrol patrol;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(50f);
        }

    }
    private void Start()
    {
        EnemiesManager.instance?.RegisterEnemy();
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        patrol = GetComponent<EnemyPatrol>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        Debug.Log("El enemigo recibio daño: " + currentHealth);  
        if (currentHealth <= maxHealth*0.2f && patrol != null)
        {
            patrol.Escape(); 
        }
        
    }

    private void Die()
    {
        isDead = true;
        Instantiate(zombie, transform.position, Quaternion.identity); 
        EnemiesManager.instance?.DeregisterEnemy();
        Destroy(gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ally"))
        {
            TakeDamage(10f); 
        }

        if (collision.CompareTag("KillEnemy"))
        {
            TakeDamage(maxHealth); 
        }
    }
}
