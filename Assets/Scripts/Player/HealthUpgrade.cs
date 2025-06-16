using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : IUpgradeEffect
{
    private float _amount;
    private int _cost;
    private int _level;
    private int _maxLevel;

    public HealthUpgrade(float amount, int cost, int level, int maxLevel = 3)
    {
        _amount = amount;
        _cost = cost;
        _level = level;
        _maxLevel = maxLevel;
    }

    public void Apply(ICanUpgrade target)
    {
        if (target is PlayerHealth health && _level < _maxLevel)
        {
            health.IncreaseMaxHealth(_amount);
        }
    }

    public int GetCost() => _cost;
    public bool CanUpgrade() => _level < _maxLevel;
    public int CurrentLevel => _level;
    public int MaxLevel => _maxLevel;
}