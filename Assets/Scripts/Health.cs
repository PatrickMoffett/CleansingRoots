using System;
using UnityEngine;


public class Health : MonoBehaviour,IDamageable
{
    public event Action<float> OnCurrentHealthChanged;
    public event Action<float> OnMaxHealthChanged;
    public event Action OnHealthIsZero;
    
    [SerializeField]
    private float maxHealth = 100f;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnCurrentHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth == 0f)
        {
            OnHealthIsZero?.Invoke();
        }
    }

    public void SetHealth(float newHealth)
    {
        _currentHealth = newHealth;
        Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnCurrentHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth == 0f)
        {
            OnHealthIsZero?.Invoke();
        }
    }

    public void AddHealth(float amountToAdd)
    {
        _currentHealth += amountToAdd;
        Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnCurrentHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth == 0f)
        {
            OnHealthIsZero?.Invoke();
            Debug.LogWarning(gameObject.name + " Health Component reached zero health after adding health.");
        }
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnMaxHealthChanged?.Invoke(newMaxHealth);
    }

    public float GetHealthPercentage()
    {
        return _currentHealth / maxHealth;
    }
}
