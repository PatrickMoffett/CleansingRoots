using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Systems.AudioManager
{
    public class MusicManager : IService
    {
        private AudioSource _currentSongSource;

        public MusicManager()
        {
            
        }

        ~MusicManager()
        {
            
        }

        public void StartSong(AudioClip songToPlay, float transitionTime,bool forceRestartSong = false)
        {
            if (!forceRestartSong && _currentSongSource &&songToPlay == _currentSongSource.clip)
            {
                return;
            }
            if (_currentSongSource == null)
            {
                _currentSongSource = ServiceLocator.Instance.Get<AudioManager>().GetMusicAudioSource();
            }
            else
            {
                if (_currentSongSource.isPlaying)
                {
                    ServiceLocator.Instance.Get<MonoBehaviorService>()
                        .StartCoroutine(FadeOutAndStopSong(_currentSongSource, transitionTime));
                    //get new Audio Source
                    _currentSongSource = ServiceLocator.Instance.Get<AudioManager>().GetMusicAudioSource();
                }
            }
            _currentSongSource.clip = songToPlay;
            _currentSongSource.loop = true;
            ServiceLocator.Instance.Get<MonoBehaviorService>().StartCoroutine(PlayAndFadeIn(_currentSongSource,transitionTime));
            _currentSongSource.Play();
        }

        IEnumerator FadeOutAndStopSong(AudioSource sourceToFade,float fadeTime)
        {
            sourceToFade.volume = 1f;
            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                sourceToFade.volume = 1-(elapsedTime / fadeTime);
                yield return null;
            }

            sourceToFade.volume = 0;
            Object.Destroy(sourceToFade);
        }
        IEnumerator PlayAndFadeIn(AudioSource sourceToFade,float fadeTime)
        {
            sourceToFade.volume = 0f;
            float elapsedTime = 0f;
            sourceToFade.Play();
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                sourceToFade.volume = elapsedTime / fadeTime;
                yield return null;
            }

            sourceToFade.volume = 1;
        }
        
    }
}