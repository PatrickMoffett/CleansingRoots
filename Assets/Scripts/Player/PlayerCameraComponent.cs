using System;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public enum PlayerCameraMode
    {
        Orbit,
        TargetLocked,
        Aiming
    }
    public class PlayerCameraComponent : MonoBehaviour
    {

        private PlayerCameraMode _currentCameraMode = PlayerCameraMode.Orbit;
        
        //Main Camera
        [SerializeField] private GameObject mainCamera;
        //Cinemachine Cameras
        [SerializeField] private GameObject orbitCamera;
        [SerializeField] private GameObject targetingCamera;
        [SerializeField] private GameObject aimingCamera;
        
        public Action<bool> aimingCameraActive;

        private void Start()
        {
            SetCurrentCameraMode(PlayerCameraMode.Orbit);
        }

        public PlayerCameraMode GetCurrentCameraMode()
        {
            return _currentCameraMode;
        }

        public void SetCurrentCameraMode(PlayerCameraMode cameraModeToSet)
        {

            switch (cameraModeToSet)
            {
                case PlayerCameraMode.Aiming:
                    orbitCamera.SetActive(false);
                    aimingCamera.SetActive(true);
                    targetingCamera.SetActive(false);
                    aimingCameraActive?.Invoke(true);
                    break;
                case PlayerCameraMode.Orbit:
                    orbitCamera.SetActive(true);
                    aimingCamera.SetActive(false);
                    targetingCamera.SetActive(false);
                    aimingCameraActive?.Invoke(false);
                    break;
                case PlayerCameraMode.TargetLocked:
                    orbitCamera.SetActive(false);
                    aimingCamera.SetActive(false);
                    targetingCamera.SetActive(true);
                    aimingCameraActive?.Invoke(false);
                    break;
                default:
                    Debug.LogError("Unsupported CameraMode");
                    break;
            }
            
            _currentCameraMode = cameraModeToSet;
        }

        public GameObject GetMainCamera()
        {
            return mainCamera;
        }
        public void SetTargetCameraLookAt(Transform transformToLookAt)
        {
            targetingCamera.GetComponent<CinemachineVirtualCamera>().LookAt = transformToLookAt;
        }
    }
}