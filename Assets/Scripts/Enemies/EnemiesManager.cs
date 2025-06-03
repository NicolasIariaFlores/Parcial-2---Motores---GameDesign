using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager instance { get; private set; }

    [SerializeField] private GameObject objectToActivate;

    private int enemiesInScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy()
    {
        enemiesInScene++;
        Debug.Log("Nuevo registro. Total: " + enemiesInScene); 
    }

    public void DeregisterEnemy()
    {
        enemiesInScene--;
        Debug.Log("Un enemigo murio. Restan: " + enemiesInScene);
        if (enemiesInScene <= 0)
        {
            Debug.Log("Todos derrotados ");
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                Debug.Log("Trigger activado");
            }
            else
            {
                Debug.LogWarning("No se asigno un objeto."); 
            }
        }
    }
}
