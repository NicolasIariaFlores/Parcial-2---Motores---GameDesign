using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuUI : MonoBehaviour
{
    [SerializeField] private Button _healthButton;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _healthCostText;

    private PlayerHealth player;
    private int _baseCost = 10;
    private int _costPerLevel = 30;
    private int _maxUpgradeLevel = 3;
    private string _upgradeId = "health";

    void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
        _healthButton.onClick.AddListener(UpgradeHealth);
        UpdateUI();
    }

    private void UpgradeHealth()
    {
        int currentLevel = UpgradeManager.Instance.GetUpgradeLevel(_upgradeId);
        int cost = _baseCost + _costPerLevel * currentLevel;

        if (!UpgradeManager.Instance.CanUpgrade(_upgradeId)) return;

        var upgrade = new HealthUpgrade(25f, cost, currentLevel, _maxUpgradeLevel);

        if (PointsSystem.Instance.TrySpend(cost))
        {
            player.ApplyUpgrade(upgrade);
            UpgradeManager.Instance.TryUpgrade(_upgradeId);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        int currentLevel = UpgradeManager.Instance.GetUpgradeLevel(_upgradeId);
        int cost = _baseCost + _costPerLevel * currentLevel;

        _healthText.text = $"Vida: {player.health} / {player.MaxHealth}";

        if (currentLevel >= _maxUpgradeLevel)
        {
            _healthButton.interactable = false;
            _healthCostText.text = "Máximo alcanzado";
        }
        else
        {
            _healthCostText.text = $"Costo: {cost} (Nivel {currentLevel}/{_maxUpgradeLevel})";
            _healthButton.interactable = true;
        }
    }
}