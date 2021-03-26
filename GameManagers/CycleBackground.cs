using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class CycleBackground : MonoBehaviour 
	{
        private GameManager_Master gameManagerMaster;
        private Animator _animator;
        private bool dark;

        private void Awake()
        {
            gameManagerMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_Master>();

            _animator = GetComponent<Animator>();
            
            dark = false;
        }

        private void Start()
        {
            _animator.SetFloat("FadeSpeed", 1 / GameManager_Master.fadeDuration);
        }

        private void OnEnable()
        {
            gameManagerMaster.CycleBackgroundEvent += ChangeBackground;
        }

        private void OnDisable()
        {
            gameManagerMaster.CycleBackgroundEvent -= ChangeBackground;
        }

        private void ChangeBackground()
        {
            if (!dark)
            {
                _animator.Play(Animator.StringToHash("BackgroundFade0To1"));
                dark = true;
            }
            else
            {
                _animator.Play(Animator.StringToHash("BackgroundFade1To0"));
                dark = false;
            }
        }
    }
}