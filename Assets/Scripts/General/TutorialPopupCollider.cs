using System;
using System.Collections.Generic;
using Systems.ApplicationStateManager.ApplicationStates;
using UnityEngine;

namespace General
{
    public class TutorialPopupCollider : MonoBehaviour
    {
        public int tutorialNumber = 0;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary["Page"] = tutorialNumber;
                ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(TutorialState),false,dictionary);
                Destroy(gameObject);
            }
        }
    }
}