using UnityEngine;

public class NPCStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;
    public float MaxHealth => _maxHealth;
    [SerializeField] private float _baseSpeed = 15f;
    public float BaseSpeed => _baseSpeed;
    [SerializeField] private float _damage = 10f;
    public float Damage => _damage;
    private float _currentHealth;
    public float CurrentHealth => _currentHealth;
    private AllyHealthBar _healthBar;

    void Start()
    {
        _currentHealth = _maxHealth;
        _healthBar = GetComponentInChildren<AllyHealthBar>();
        if (_healthBar != null)
        {
            _healthBar.Initialize(transform); // posiciona la barra
            _healthBar.UpdateHealth(_currentHealth, _maxHealth); // valor inicial
        }
        else
        {
            Debug.LogWarning("No se encontró AllyHealthBar en el aliado.");
        }
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        
        if (_healthBar != null)
        {
            _healthBar.UpdateHealth(_currentHealth, _maxHealth);
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} murió.");
        gameObject.SetActive(false);
    }

    void Update()
{
    // Test temporal: bajar vida con tecla H
    if (Input.GetKeyDown(KeyCode.H))
    {
        TakeDamage(10f);
    }
}
}
