using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private float maxHealth;
    private float health;

    private void Awake()
    {
        health = maxHealth; 
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("RECIBIO DAÑO " + health); 
        if (health <= 0)
        {
            Die(); 
        }    
    }

    private void Die()
    {
        Debug.Log("MURIO"); 
    }
}
