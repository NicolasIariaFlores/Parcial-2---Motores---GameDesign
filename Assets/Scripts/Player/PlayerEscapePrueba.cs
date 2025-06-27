using UnityEngine;

public class PlayerEscapePrueba : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    [SerializeField] private SceneLoader sceneLoader;
    [Tooltip ("Si esta activado, el player debe matar a todos los enemigos que vayan a spawnear para escapar.")]
    [SerializeField] private bool killThemAll; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (killThemAll && EnemiesSpawner.Instance != null && !EnemiesSpawner.Instance.AllEnemiesDefeated())
            {
                Debug.Log("Falta matar enemigos.");
                return;
            }

            PlayerHealth currentHealth = collision.GetComponent<PlayerHealth>();

            if (currentHealth != null && GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth = (int)currentHealth.health;              
            }
            sceneLoader.LoadSceneByNumber(sceneToLoad); 
        }
    }
}
