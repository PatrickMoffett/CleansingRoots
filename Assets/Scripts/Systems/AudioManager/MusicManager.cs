using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Systems.AudioManager
{
    public class MusicManager : IService
    {
        private AudioSource _currentSongSource;
        private float _fadeOutTime = 1f;

        public MusicManager()
        {
            
        }

        ~MusicManager()
        {
            
        }

        public void StartSong(AudioClip songToPlay)
        {
            if (_currentSongSource == null)
            {
                _currentSongSource = ServiceLocator.Instance.Get<AudioManager>().GetMusicAudioSource();
            }
            else
            {
                if (_currentSongSource.isPlaying)
                {
                    ServiceLocator.Instance.Get<MonoBehaviorService>()
                        .StartCoroutine(FadeAndStopSong(_currentSongSource, _fadeOutTime));
                }
            }
            _currentSongSource.clip = songToPlay;
            _currentSongSource.loop = true;
            _currentSongSource.Play();
        }

        IEnumerator FadeAndStopSong(AudioSource sourceToFade,float time)
        {
            //TODO: Fade audio before deleting
            yield return null;
            Object.Destroy(sourceToFade);
        }
        
        
    }
}