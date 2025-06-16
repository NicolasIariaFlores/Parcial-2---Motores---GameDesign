using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    private Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();
    private const int MaxUpgradeLevel = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanUpgrade(string upgradeId)
    {
        return !_upgradeLevels.ContainsKey(upgradeId) || _upgradeLevels[upgradeId] < MaxUpgradeLevel;
    }

    public int GetUpgradeLevel(string upgradeId)
    {
        if (_upgradeLevels.TryGetValue(upgradeId, out int level))
            return level;
        return 0;
    }

    public bool TryUpgrade(string upgradeId)
    {
        if (!CanUpgrade(upgradeId))
            return false;

        _upgradeLevels.TryGetValue(upgradeId, out int currentLevel);
        _upgradeLevels[upgradeId] = currentLevel + 1;
        return true;
    }

    public int GetMaxUpgradeLevel() => MaxUpgradeLevel;
}