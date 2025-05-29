using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth; 
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; 

        currentHealth -= amount;

        if (currentHealth <= 0)
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
