
using System.Collections;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float cd;

    private Coroutine damageRoutine;
    private Collider2D currentTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidTarget(collision) && damageRoutine == null)
        {
            currentTarget = collision;
            damageRoutine = StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == currentTarget)
        {
            StopCoroutine(damageRoutine);
            damageRoutine = null;
            currentTarget = null;
        }
    }

    private bool IsValidTarget(Collider2D collision)
    {
        return collision.CompareTag("Player") || collision.CompareTag("Ally");
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            if (currentTarget != null)
            {
                DoDamage(currentTarget);
            }
            yield return new WaitForSeconds(cd);
        }
    }

    private void DoDamage(Collider2D collision)
    {
        if (!IsValidTarget(collision)) return;

        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        NPCStats npcStats = collision.GetComponent<NPCStats>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Daño aplicado a Player");
        }
        else if (npcStats != null)
        {
            npcStats.TakeDamage(damage);
            Debug.Log("Daño aplicado a Ally");
        }
    }
}
