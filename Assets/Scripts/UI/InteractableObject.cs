using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private GameObject _pressIndicator;
    [SerializeField] private KeyCode _interactionKey;

    private bool _playerInRange = false;
    private IInteractable _interactable;

    private void Start()
    {
        if (_pressIndicator != null)
            _pressIndicator.SetActive(false);

        _interactable = GetComponent<IInteractable>();
        //if (_interactable == null)
        //    Debug.LogWarning($"{gameObject.name} no implementa IInteractable.");
    }

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(_interactionKey))
        {
            _interactable?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = true;
            if (_pressIndicator != null)
                _pressIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = false;
            if (_pressIndicator != null)
                _pressIndicator.SetActive(false);
        }
    }
}