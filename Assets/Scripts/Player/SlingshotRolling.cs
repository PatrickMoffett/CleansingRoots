using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotRolling : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.right,90.0f*Time.deltaTime);
    }
}
