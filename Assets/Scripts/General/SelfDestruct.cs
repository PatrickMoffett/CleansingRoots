using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destroyActorSec = 4.0f;
    void Start()
    {
        Destroy(gameObject, destroyActorSec);
    }

    
    
}
