using System;
using UnityEngine;

namespace Systems.PlayerManager
{
    public class PlayerManager : IService
    {
        public Action playerRegistered;
        public Action playerUnregistered;

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
                _player = null;
                playerUnregistered?.Invoke();
            }
        }

        public GameObject GetPlayer()
        {
            return _player;
        }
    }
}