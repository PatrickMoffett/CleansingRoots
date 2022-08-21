using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

namespace Systems.AudioManager
{
    [CreateAssetMenu(menuName="ScriptableObject/Settings/AudioManagerParams")]
    public class AudioManagerParams : ScriptableObject
    {
        public AudioMixer mixer;
        public AudioMixerGroup masterGroup;
        public AudioMixerGroup sfxGroup;
        public AudioMixerGroup musicGroup;
    }
    public class AudioManager : IService
    {
        //params set from the editor
        private readonly AudioManagerParams _params = Resources.Load<AudioManagerParams>("AudioManagerParams");
        
        private readonly AudioMixer _mixer;
        
        //source for playing non locational sfx
        private readonly AudioSource _sfxAudioSource;
        
        //gameobject to organize spawned audiosources
        private readonly GameObject _bucket;
        public AudioManager()
        {
            _mixer = _params.mixer;
        
            _bucket = new GameObject("AudioBucket");
            
            _sfxAudioSource = _bucket.AddComponent<AudioSource>();
            _sfxAudioSource.outputAudioMixerGroup = _params.sfxGroup;
            
            Object.DontDestroyOnLoad(_bucket);
        }

        ~AudioManager()
        {
            Object.Destroy(_bucket);
        }

        public float GetMasterVolume()
        {
            _mixer.GetFloat("MasterVolume", out float volume);
            return DecibelToLinear(volume);
        }
        public float GetMusicVolume()
        {
            _mixer.GetFloat("MusicVolume", out float volume);
            return DecibelToLinear(volume);
        }
        public float GetSFXVolume()
        {
            _mixer.GetFloat("SFXVolume", out float volume);
            return DecibelToLinear(volume);
        }
        public void SetMasterVolume(float newVal)
        {
            newVal = Mathf.Clamp(newVal, 0f, 1f);
            newVal = LinearToDecibel(newVal);
            _mixer.SetFloat("MasterVolume", newVal);
        }

        public void SetMusicVolume(float newVal)
        {
            newVal = Mathf.Clamp(newVal, 0f, 1f);
            newVal = LinearToDecibel(newVal);
            _mixer.SetFloat("MusicVolume", newVal);
        }

        public void SetSFXVolume(float newVal)
        {
            newVal = Mathf.Clamp(newVal, 0f, 1f);
            newVal = LinearToDecibel(newVal);
            _mixer.SetFloat("SFXVolume", newVal);
        }
        private float LinearToDecibel(float linear)
        {
            float dB;

            if (linear != 0)
                dB = 20.0f * Mathf.Log10(linear);
            else
                dB = -144.0f;

            return dB;
        }

        private float DecibelToLinear(float dB)
        {
            float linear = Mathf.Pow(10.0f, dB / 20.0f);

            return linear;
        }

        public void PlaySFXAtLocation(AudioClip clipToPlay, Vector3 location)
        {
        
        }

        public void PlaySFX(AudioClip clipToPlay)
        {
            _sfxAudioSource.PlayOneShot(clipToPlay);
        }

        public AudioSource GetMusicAudioSource()
        {
            AudioSource newSource = _bucket.AddComponent<AudioSource>();
            newSource.outputAudioMixerGroup = _params.musicGroup;
            return newSource;
        }
    }
}