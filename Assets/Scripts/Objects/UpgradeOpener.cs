using UnityEngine;

public class UpgradeOpener : MonoBehaviour
{
    [SerializeField] private GameObject upgradeMenuCanvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            upgradeMenuCanvas.SetActive(true);
            Time.timeScale = 0; // Pausa el juego si querés
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            upgradeMenuCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }
}