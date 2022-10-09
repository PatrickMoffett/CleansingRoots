using Systems.AudioManager;
using Unity.Mathematics;
using UnityEngine;

namespace Interactables.Switches
{
    public class SelfDestructSwitch : MonoBehaviour, IDamageable
    {
        public RespondsToSwitch objectToSwitch;
        public GameObject BrokenVersion;
        public GameObject VFX;
        public AudioClip SFX;
        
        
    
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
            Instantiate(VFX, transform.position, Quaternion.identity);
            Instantiate(BrokenVersion, transform.position, Quaternion.identity);
            ServiceLocator.Instance.Get<AudioManager>().PlaySFX(SFX);
            objectToSwitch.ToggleSwitch();
            Destroy(gameObject);
        }
    }
}
