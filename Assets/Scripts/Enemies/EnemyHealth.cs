using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameObject zombie;
    [SerializeField] private AllyHealthBar healthBar; 
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
        HealthBarControl(); 
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        Debug.Log("El HUMANO recibio daño: " + currentHealth);  
        if (currentHealth <= maxHealth*0.2f && patrol != null)
        {
            patrol.Escape(); 
        }
        
    }
    
    private void HealthBarControl()
    {
        if (healthBar != null)
        {
            healthBar.Initialize(transform);
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Die()
{
    isDead = true;

    // Instanciar Chombi
    GameObject newAlly = Instantiate(zombie, transform.position, Quaternion.identity);

    // Ver nuevo chombi
    if (newAlly.TryGetComponent(out FollowPlayer follow))
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
                follow.SetPlayer(player);
        }
    }

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