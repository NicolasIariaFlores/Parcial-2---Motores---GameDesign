
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, ICanUpgrade
{
    [SerializeField] private float _maxHealth;
    public float MaxHealth => _maxHealth;
    private float _health;
    public float health => _health;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;

    [SerializeField] private AudioClip takeDamageAudio;
    private AudioSource audioSource; 

    [SerializeField] private int sceneToLoad;
    [SerializeField] private SceneLoader sceneLoader;

    private void Awake()
    {
        DontDestroyOnLoad(transform.root.gameObject);
        InitPlayer();

        if (GameManager.Instance != null && GameManager.Instance.playerHealth > 0)
        {
            _health = GameManager.Instance.playerHealth;
        }
    }

    void InitPlayer()
    {
        _health = _maxHealth;

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; 
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        { 
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;

        if(audioSource != null && takeDamageAudio != null) 
        {
            audioSource.PlayOneShot(takeDamageAudio);
        }
        
        Debug.Log("RECIBIO DAÑO " + _health);

        StartCoroutine(FlashEffect());

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

    private IEnumerator FlashEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }
}
