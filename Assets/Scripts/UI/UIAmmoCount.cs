using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Player;

public class UIAmmoCount : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private TextMeshProUGUI ammoText;
    
    private PlayerCharacter _playerCharacter;
    private int _currentAmmo;
    private int _maxAmmo;

    void Start() 
    { 
        _playerCharacter = _gameObject.GetComponent<PlayerCharacter>();
        _currentAmmo = _playerCharacter.GetCurrentAmmo();
        _maxAmmo = _playerCharacter.GetMaxAmmo();
    }

    void Update() 
    {
        ammoText.text = _currentAmmo.ToString() + "/" + _maxAmmo.ToString();
    }
}
