using UnityEngine;

public class PlayerEscapePrueba : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private string spawnPoint = "SpawnPoint_City"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth currentHealth = collision.GetComponent<PlayerHealth>();

            if (currentHealth != null && GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth = (int)currentHealth.health;
               
                GameManager.Instance.spawnPoint = spawnPoint;
            }
            sceneLoader.LoadSceneByNumber(sceneToLoad); 
        }
    }
}
