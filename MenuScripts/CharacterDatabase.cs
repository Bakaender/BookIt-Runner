using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class CharacterDatabase : MonoBehaviour 
	{
        public Sprite[] InspectorBookSprites = new Sprite[1];
        //public Sprite[] InspectorHatSprites = new Sprite[1];
        public AnimatorOverrideController[] InspectorCharacterAnimationOverrides = new AnimatorOverrideController[1];

        public static Sprite[] BookSprites;
        //public static Sprite[] HatSprites;
        public static AnimatorOverrideController[] CharacterAnimations;

        private void Awake()
        {
            BookSprites = new Sprite[InspectorBookSprites.Length];
            for (int i = 0; i < InspectorBookSprites.Length; i++)
            {
                BookSprites[i] = InspectorBookSprites[i];
            }

            //HatSprites = new Sprite[InspectorHatSprites.Length];
            //for (int i = 0; i < InspectorHatSprites.Length; i++)
            //{
            //    HatSprites[i] = InspectorHatSprites[i];
            //}

            CharacterAnimations = new AnimatorOverrideController[InspectorCharacterAnimationOverrides.Length];
            for (int i = 0; i < InspectorCharacterAnimationOverrides.Length; i++)
            {
                CharacterAnimations[i] = InspectorCharacterAnimationOverrides[i];
            }
        }
    }
}

