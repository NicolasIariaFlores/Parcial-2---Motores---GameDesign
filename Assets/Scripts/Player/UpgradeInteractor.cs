using UnityEngine;

public class InteractorUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    private bool _playerInRange = false;

    void Update()
    {
        if (_playerInRange && Input.GetKeyDown(interactionKey))
        {
            upgradeCanvas.SetActive(!upgradeCanvas.activeSelf);
            Time.timeScale = upgradeCanvas.activeSelf ? 0 : 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = false;
            upgradeCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }
}