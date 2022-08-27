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
}