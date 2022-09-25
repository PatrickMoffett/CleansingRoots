using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SyncText : MonoBehaviour
{
    private TMP_Text _frontText; 
    [SerializeField] private TMP_Text _backText; 

    void Start()
    {
        _frontText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _backText.text = _frontText.text;
    }
}
