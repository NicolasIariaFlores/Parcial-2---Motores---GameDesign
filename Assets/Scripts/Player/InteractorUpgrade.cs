using UnityEngine;

public class UpgradeInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject upgradeCanvas;

    public void Interact()
    {
        bool isActive = upgradeCanvas.activeSelf;
        upgradeCanvas.SetActive(!isActive);
        Time.timeScale = isActive ? 1 : 0;
    }
}