using System;
using System.Collections;
using System.Collections.Generic;
using Systems.PlayerManager;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIHealthbar : MonoBehaviour
{
    
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Image _image;

    private Health _health;
    private float _currentHealth;
    private float _maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        ServiceLocator.Instance.Get<PlayerManager>().playerRegistered += Setup;
        if (_health != null)
        {
            _health.OnCurrentHealthChanged += UpdateHealth;
        }
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.Get<PlayerManager>().playerRegistered -= Setup;
        _health.OnCurrentHealthChanged -= UpdateHealth;
    }

    void Setup()
    {
        //setup health
        GameObject player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
        _health = player.GetComponent<Health>();
        Assert.IsNotNull(_health);
        _health.OnCurrentHealthChanged += UpdateHealth;
        UpdateHealth(_health.GetCurrentHealth());
    }

    void UpdateHealth(int newHealth)
    {
        _image.fillAmount = _health.GetHealthPercentage();
    }
}
