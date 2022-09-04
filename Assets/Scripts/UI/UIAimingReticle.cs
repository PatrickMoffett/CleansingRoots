using System.Collections.Generic;
using Player;
using Systems.PlayerManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIAimingReticle : MonoBehaviour
    {
        public List<Image> Reticle;

        void Start()
        {
            ServiceLocator.Instance.Get<PlayerManager>().playerRegistered += Setup;
            ShowReticle(false);
        }

        void Setup()
        {
            //setup health
            GameObject player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
            PlayerCameraComponent pcc = player.GetComponent<PlayerCameraComponent>();
            pcc.aimingCameraActive += ShowReticle;
        }

        private void ShowReticle(bool show)
        {
            foreach (var image in Reticle)
            {
                image.enabled = show;
            }
        }
    }
}