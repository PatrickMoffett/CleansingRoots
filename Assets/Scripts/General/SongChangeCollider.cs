using System;
using Systems.AudioManager;
using UnityEngine;
using UnityEngine.Assertions;

namespace General
{
    public class SongChangeCollider : MonoBehaviour
    {
        public AudioClip songToPlay;
        public float fadeTime = 1f;

        private void OnTriggerEnter(Collider other)
        {
            Assert.IsNotNull(songToPlay);
            if (other.CompareTag("Player"))
            {
                ServiceLocator.Instance.Get<MusicManager>().StartSong(songToPlay,fadeTime);
            }
        }
    }
}