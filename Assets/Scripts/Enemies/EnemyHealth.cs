
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    
    [SerializeField] private float maxHealth;
    [SerializeField]private float currentHealth;
    private bool isDead = false;
    private EnemyPatrol patrol;

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
        EnemiesManager.instance?.DeregisterEnemy();
        //LOGICA DE MUERTE 
        Destroy(gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ally"))
        {
            TakeDamage(10f); 
        }

        if (collision.CompareTag("HumanLimits"))
        {
            TakeDamage(maxHealth); 
        }
    }
}
