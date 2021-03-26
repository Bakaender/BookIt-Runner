using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BookIt
{
    public class MusicChanger : MonoBehaviour
    {
        public float fadeOutTime;
        public float fadeInTime;

        public AudioClip menuMusic;
        public AudioClip gameplayMusic;
        public AudioMixerGroup mixerGroup;

        private AudioSource musicSource;
        private bool fadingOut;
        private bool fadingIn;

        private void Start()
        {
            //musicSource = GetComponent<AudioSource>();
            fadingIn = false;
            fadingOut = false;

            musicSource = AddSource(menuMusic, mixerGroup, true, true, 1f);
            musicSource.Play();
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

        public void FadeMenuToGameplay()
        {
            StartCoroutine(FadeOut(gameplayMusic));
        }

        public void FadeGameplayToMenu()
        {
            StartCoroutine(FadeOut(menuMusic));
        }

        private void Update()
        {
            if (fadingOut)
            {
                musicSource.volume -= 1 / fadeOutTime * Time.deltaTime;
            }
            if (fadingIn)
            {
                musicSource.volume += 1 / fadeInTime * Time.deltaTime;
            }
        }

        IEnumerator FadeOut(AudioClip fadeTo)
        {
            fadingOut = true;

            yield return new WaitForSeconds(fadeOutTime);

            fadingOut = false;
            musicSource.clip = fadeTo;
            StartCoroutine(FadeIn());
        }

        IEnumerator FadeIn()
        {
            musicSource.Play();
            fadingIn = true;

            yield return new WaitForSeconds(fadeInTime);

            fadingIn = false;
            musicSource.volume = 1;          
        }
    }
}

