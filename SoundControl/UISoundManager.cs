using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BookIt
{
	public class UISoundManager : MonoBehaviour 
	{
        public AudioClip buttonPress;
        public AudioClip bookUnlock;
        public AudioMixerGroup mixerGroup;

        private AudioSource uiSFXSource;

        void Start() 
		{
            uiSFXSource = AddSource(null, mixerGroup, false, false, 1f);
		}

        public AudioSource AddSource(AudioClip clip, AudioMixerGroup mixerGroup, bool loop, bool playAwake, float vol)
        {
            AudioSource newAudio = gameObject.AddComponent<AudioSource>();
            newAudio.clip = clip;
            newAudio.outputAudioMixerGroup = mixerGroup;
            newAudio.loop = loop;
            newAudio.playOnAwake = playAwake;
            newAudio.volume = vol;
            return newAudio;
        }

        public void PlayButtonPressSound()
        {
            uiSFXSource.PlayOneShot(buttonPress);
        }

        public void PlayBookUnlockSound()
        {
            uiSFXSource.PlayOneShot(bookUnlock);
        }
    }
}

