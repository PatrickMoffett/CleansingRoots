using Player;
using Systems.PlayerManager;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIAmmoCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoText;
    
        private PlayerCharacter _playerCharacter;

        void Start() 
        { 
            ServiceLocator.Instance.Get<PlayerManager>().playerRegistered += Setup;
        }

        void Setup()
        {
            //setup ammo
            GameObject player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
            _playerCharacter = player.GetComponent<PlayerCharacter>();
            _playerCharacter.playerAmmoChanged += UpdateAmmo;
            UpdateAmmo(_playerCharacter.GetCurrentAmmo());
        }

        private void UpdateAmmo(int ammo)
        {
            ammoText.text = _playerCharacter.GetCurrentAmmo().ToString() + "/" + _playerCharacter.GetMaxAmmo().ToString();
        }
    }
}
