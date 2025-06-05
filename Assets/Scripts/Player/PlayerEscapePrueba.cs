using UnityEngine;

public class PlayerEscapePrueba : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    [SerializeField] private SceneLoader sceneLoader; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sceneLoader.LoadSceneByNumber(sceneToLoad); 
        }
    }
}
