
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerHealth;

    //TODO
    //CARGAR LISTA DE ZOMBIES 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instanciado y persiste.");
        }
        else
        {
            Debug.LogWarning("GameManager duplicado, se destruye.");
            Destroy(gameObject);
        }
    }
}
