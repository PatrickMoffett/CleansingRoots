using System;
using UnityEngine;

namespace General
{
    public class DieOnUpdate : MonoBehaviour
    {
        private void Update()
        {
            Destroy(gameObject);
        }
    }
}