
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    
    [SerializeField] private float maxHealth;
    [SerializeField]private float currentHealth;
    private bool isDead = false;
    private EnemyPatrol patrol; 

    private void Awake()
    {
        currentHealth = maxHealth;
        patrol = GetComponent<EnemyPatrol>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; 

        currentHealth -= amount;

        if (currentHealth <= maxHealth*0.2f && patrol != null)
        {
            patrol.Escape(); 
        }
        else if (currentHealth <= 0)
        {
            Die(); 
        }
    }

    
    private void Die()
    {
        isDead = true; 
        //LOGICA DE MUERTE 
        Destroy(gameObject); 
    }
}
