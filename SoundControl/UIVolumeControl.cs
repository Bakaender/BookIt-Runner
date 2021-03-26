using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace BookIt
{
	public class UIVolumeControl : MonoBehaviour 
	{
        public AudioMixer masterAudioMixer;
        public AudioMixer playerSFXAudioMixer;
        public float sliderVolumeMiddleLevel;

        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider SFXVolumeSlider;
        public Slider runVolumeSlider;
        public Slider glideVolumeSlider;

        public Slider pausedMasterVolumeSlider;
        public Slider pausedMusicVolumeSlider;
        public Slider pausedSFXVolumeSlider;
        public Slider pausedRunVolumeSlider;
        public Slider pausedGlideVolumeSlider;

        private void Start()
        {
            SetSliderValues();
        }

        public void SetSliderValues()
        {
            masterVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.masterVolume);
            musicVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.musicVolume);
            SFXVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.masterSFXVolume);
            runVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.runVolume);
            glideVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.glideVolume);

            pausedMasterVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.masterVolume);
            pausedMusicVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.musicVolume);
            pausedSFXVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.masterSFXVolume);
            pausedRunVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.runVolume);
            pausedGlideVolumeSlider.value = CalculateSliderValue(NewSaveGame.Instance.glideVolume);
        }

        private float CalculateSliderValue(float value)
        {
            if (value < sliderVolumeMiddleLevel)
            {

                return ((value - -80f) / (sliderVolumeMiddleLevel - -80f)) - 1;
            }
            else if (value == sliderVolumeMiddleLevel)
            {
                return 0f;
            }
            else
            {
                return ((value - sliderVolumeMiddleLevel) / (sliderVolumeMiddleLevel)) * -1f;
            }
        }

        public void SetMasterVolume(float level)
        {
            if (level < 0)
            {
                float newLevel = sliderVolumeMiddleLevel - ((-80f - sliderVolumeMiddleLevel) * level);
                masterAudioMixer.SetFloat("MasterVolume", newLevel);
                NewSaveGame.Instance.masterVolume = newLevel;
            }
            else if (level == 0)
            {
                masterAudioMixer.SetFloat("MasterVolume", sliderVolumeMiddleLevel);
                NewSaveGame.Instance.masterVolume = sliderVolumeMiddleLevel;
            }
            else if (level > 0)
            {
                float newLevel = sliderVolumeMiddleLevel - (sliderVolumeMiddleLevel * level);
                masterAudioMixer.SetFloat("MasterVolume", newLevel);
                NewSaveGame.Instance.masterVolume = newLevel;
            }
        }

        public void SetMusicVolume(float level)
        {
            if (level < 0)
            {
                float newLevel = sliderVolumeMiddleLevel - ((-80f - sliderVolumeMiddleLevel) * level);
                masterAudioMixer.SetFloat("MusicVolume", newLevel);
                NewSaveGame.Instance.musicVolume = newLevel;
            }
            else if (level == 0)
            {
                masterAudioMixer.SetFloat("MusicVolume", sliderVolumeMiddleLevel);
                NewSaveGame.Instance.musicVolume = sliderVolumeMiddleLevel;
            }
            else if (level > 0)
            {
                float newLevel = sliderVolumeMiddleLevel - (sliderVolumeMiddleLevel * level);
                masterAudioMixer.SetFloat("MusicVolume", newLevel);
                NewSaveGame.Instance.musicVolume = newLevel;
            }
        }

        public void SetSFXVolume(float level)
        {
            if (level < 0)
            {
                float newLevel = sliderVolumeMiddleLevel - ((-80f - sliderVolumeMiddleLevel) * level);
                masterAudioMixer.SetFloat("SFXVolume", newLevel);
                NewSaveGame.Instance.masterSFXVolume = newLevel;
            }
            else if (level == 0)
            {
                masterAudioMixer.SetFloat("SFXVolume", sliderVolumeMiddleLevel);
                NewSaveGame.Instance.masterSFXVolume = sliderVolumeMiddleLevel;
            }
            else if (level > 0)
            {
                float newLevel = sliderVolumeMiddleLevel - (sliderVolumeMiddleLevel * level);
                masterAudioMixer.SetFloat("SFXVolume", newLevel);
                NewSaveGame.Instance.masterSFXVolume = newLevel;
            }
        }

        public void SetRunVolume(float level)
        {
            if (level < 0)
            {
                float newLevel = sliderVolumeMiddleLevel - ((-80f - sliderVolumeMiddleLevel) * level);
                playerSFXAudioMixer.SetFloat("RunVolume", newLevel);
                NewSaveGame.Instance.runVolume = newLevel;
            }
            else if (level == 0)
            {
                playerSFXAudioMixer.SetFloat("RunVolume", sliderVolumeMiddleLevel);
                NewSaveGame.Instance.runVolume = sliderVolumeMiddleLevel;
            }
            else if (level > 0)
            {
                float newLevel = sliderVolumeMiddleLevel - (sliderVolumeMiddleLevel * level);
                playerSFXAudioMixer.SetFloat("RunVolume", newLevel);
                NewSaveGame.Instance.runVolume = newLevel;
            }
        }

        public void SetGlideVolume(float level)
        {
            if (level < 0)
            {
                float newLevel = sliderVolumeMiddleLevel - ((-80f - sliderVolumeMiddleLevel) * level);
                playerSFXAudioMixer.SetFloat("GlideVolume", newLevel);
                NewSaveGame.Instance.glideVolume = newLevel;
            }
            else if (level == 0)
            {
                playerSFXAudioMixer.SetFloat("GlideVolume", sliderVolumeMiddleLevel);
                NewSaveGame.Instance.glideVolume = sliderVolumeMiddleLevel;
            }
            else if (level > 0)
            {
                float newLevel = sliderVolumeMiddleLevel - (sliderVolumeMiddleLevel * level);
                playerSFXAudioMixer.SetFloat("GlideVolume", newLevel);
                NewSaveGame.Instance.glideVolume = newLevel;
            }
        }
    }
}

