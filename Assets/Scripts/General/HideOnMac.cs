using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnMac : MonoBehaviour
{
#if UNITY_STANDALONE_OSX
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject);
    }
#endif
    // Update is called once per frame
    void Update()
    {
        
    }
}
