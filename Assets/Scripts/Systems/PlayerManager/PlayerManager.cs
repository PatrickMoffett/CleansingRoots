using System;
using UnityEngine;

namespace Systems.PlayerManager
{
    public class PlayerManager : IService
    {
        public event Action playerRegistered;
        public event Action playerUnregistered;

        private GameObject _player;
        
        public void RegisterPlayer(GameObject player)
        {
            _player = player;
            playerRegistered?.Invoke();
        }

        public void UnregisterPlayer(GameObject player)
        {
            if (player == _player)
            {
                playerUnregistered?.Invoke();
                _player = null;
            }
        }

        public GameObject GetPlayer()
        {
            return _player;
        }
    }
}