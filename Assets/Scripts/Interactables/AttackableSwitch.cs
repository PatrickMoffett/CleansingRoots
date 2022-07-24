using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableSwitch : MonoBehaviour, IDamageable
{
    public RespondsToSwitch ObjectToSwitch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        ObjectToSwitch.ToggleSwitch();
    }
}
