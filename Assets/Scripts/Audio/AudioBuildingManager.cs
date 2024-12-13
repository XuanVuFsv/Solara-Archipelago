using UnityEngine;
using VitsehLand.Scripts.Pattern.Singleton;

namespace VitsehLand.Scripts.Audio
{
    public class AudioBuildingManager : Singleton<AudioBuildingManager>
    {
        public AudioClip activeClip, switchSound, enemyAttack, enemyDie, breakCrystal, breakShield, punch;
        public AudioSource audioSource, walk, suckUpSound;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayAudioClip(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.PlayOneShot(audioSource.clip);
        }

        public void StopPlayAudio()
        {
            audioSource.Stop();
        }
    }

}