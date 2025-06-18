using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float cd;
    private float lastDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoDamage(collision); 
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        DoDamage(collision); 
    }

    private void DoDamage(Collider2D collision)
    {
        if (collision.CompareTag("Player") /*|| collision.CompareTag("Ally")*/ && Time.time - lastDamage >= cd)
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                lastDamage = Time.time;
            }
        }
    }
}
