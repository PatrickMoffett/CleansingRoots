using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIHealthbar : MonoBehaviour
{
    
    [Header("Healthbar Properties")]
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Image _image;

    private Health _health;
    private float _currentHealth;
    private float _maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        _health = _gameObject.GetComponent<Health>();
        Assert.IsNotNull(_health);
    }

    // Update is called once per frame
    void Update()
    {
        _currentHealth = _health.GetCurrentHealth();
        _maxHealth = _health.GetMaxHealth();
        _image.fillAmount = _currentHealth / _maxHealth;
    }
}
