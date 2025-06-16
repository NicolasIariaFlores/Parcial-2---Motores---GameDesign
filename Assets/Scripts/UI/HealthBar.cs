using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image rellenoBarraVida;
    private PlayerHealth playerHealth;
    private float maxHealth;

    void Start()
    {
        playerHealth = GameObject.Find("Zombiesito").GetComponent<PlayerHealth>();
        maxHealth = playerHealth.health;
    }

        void Update()
    {
        rellenoBarraVida.fillAmount = playerHealth.health / playerHealth.MaxHealth;
    }
}