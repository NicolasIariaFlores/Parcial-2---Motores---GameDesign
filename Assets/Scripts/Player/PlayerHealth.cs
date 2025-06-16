
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, ICanUpgrade
{
    [SerializeField] private float _maxHealth;
    public float MaxHealth => _maxHealth;
    private float _health;
    public float health => _health;

    [SerializeField] private int sceneToLoad;
    [SerializeField] private SceneLoader sceneLoader;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        Debug.Log("RECIBIO DAÑO " + _health);
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        sceneLoader.LoadSceneByNumber(sceneToLoad);
        Debug.Log("MURIO");
    }

    public void IncreaseMaxHealth(float amount)
    {
        _maxHealth += amount;
        _health += amount;
    }

    public void ApplyUpgrade(IUpgradeEffect upgrade)
    {
        upgrade.Apply(this);
    }
}
