using System;
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

        private PlayerCameraComponent pcc;

        void Start()
        {
            ShowReticle(false);
        }

        private void Teardown()
        {
            GameObject player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
            pcc = player.GetComponent<PlayerCameraComponent>();
            pcc.aimingCameraActive -= ShowReticle;
        }

        void Setup()
        {
            GameObject player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
            pcc = player.GetComponent<PlayerCameraComponent>();
            pcc.aimingCameraActive += ShowReticle;
        }
        private void OnEnable()
        {
            ServiceLocator.Instance.Get<PlayerManager>().playerRegistered += Setup;
            ServiceLocator.Instance.Get<PlayerManager>().playerUnregistered += Teardown;
            if (pcc == null) return;
            pcc.aimingCameraActive += ShowReticle;
        }

        private void OnDisable()
        {
            pcc.aimingCameraActive -= ShowReticle;
            ServiceLocator.Instance.Get<PlayerManager>().playerRegistered -= Setup;
            ServiceLocator.Instance.Get<PlayerManager>().playerUnregistered -= Teardown;
        }

        private void ShowReticle(bool show)
        {
            foreach (var image in Reticle)
            {
                //if (image == null) continue;
                image.enabled = show;
            }
        }
    }
}