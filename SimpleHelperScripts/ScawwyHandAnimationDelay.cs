using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class ScawwyHandAnimationDelay : MonoBehaviour 
	{
        private Animator animator;
        public float delay;

        private void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            animator.SetFloat("Speed", 0);
            Invoke("PlayAnimation", delay);
        }

        void PlayAnimation()
        {
            animator.SetFloat("Speed", 1);
        }
    }
}

