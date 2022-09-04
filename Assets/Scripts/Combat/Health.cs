using System;
using UnityEngine;


public class Health : MonoBehaviour,IDamageable
{
    public event Action<int> OnCurrentHealthChanged;
    public event Action<int> OnMaxHealthChanged;
    public event Action OnHealthIsZero;
    
    [SerializeField]
    private int maxHealth = 10;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;            
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnCurrentHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth == 0f)
        {
            Debug.Log("Health is Zero");
            OnHealthIsZero?.Invoke();
        }
    }

    public void SetHealth(int newHealth)
    {
        _currentHealth = newHealth;
        Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnCurrentHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth == 0f)
        {
            Debug.Log("Health is Zero");
            OnHealthIsZero?.Invoke();
        }
    }

    public void AddHealth(int amountToAdd)
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

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnMaxHealthChanged?.Invoke(newMaxHealth);
    }

    public float GetHealthPercentage()
    {
        return (float)_currentHealth / maxHealth;
    }
}
