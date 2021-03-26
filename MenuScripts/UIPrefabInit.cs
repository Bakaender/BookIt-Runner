using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class UIPrefabInit : MonoBehaviour 
	{
        public Animator prefab1Animator;
        public Animator prefab2Animator;
        public Animator prefab3Animator;
        //public SpriteRenderer prefab1HatSprite;
        //public SpriteRenderer prefab2HatSprite;
        //public SpriteRenderer prefab3HatSprite;

        //public SpriteRenderer previewHatSprite;

        private int usedBook;
        //private int usedHat;
        private string[] prefabSplit;

        private void Start()
        {
            //Init prefab 1
            prefabSplit = NewSaveGame.Instance.prefabs[0].Split(',');
            int.TryParse(prefabSplit[0], out usedBook);
            //int.TryParse(prefabSplit[1], out usedHat);
            UpdatePrefabBook(0, usedBook);
            //UpdatePrefabHat(0, usedHat);

            //Init prefab 2
            prefabSplit = NewSaveGame.Instance.prefabs[1].Split(',');
            int.TryParse(prefabSplit[0], out usedBook);
            //int.TryParse(prefabSplit[1], out usedHat);
            UpdatePrefabBook(1, usedBook);
            //UpdatePrefabHat(1, usedHat);

            //Init prefab 3
            prefabSplit = NewSaveGame.Instance.prefabs[2].Split(',');
            int.TryParse(prefabSplit[0], out usedBook);
            //int.TryParse(prefabSplit[1], out usedHat);
            UpdatePrefabBook(2, usedBook);
            //UpdatePrefabHat(2, usedHat);
        }

        /// <summary>
        /// Updates book skin
        /// </summary>
        /// <param name="prefab">0-2 is prefab 1-3.</param>
        /// <param name="book">int corresponding to CharacterDatabase CharacterAnimations Array.</param>
        public void UpdatePrefabBook(int prefab, int book)
        {
            if (prefab == 0)
            {
                prefab1Animator.runtimeAnimatorController = CharacterDatabase.CharacterAnimations[book];
            }
            else if (prefab == 1)
            {
                prefab2Animator.runtimeAnimatorController = CharacterDatabase.CharacterAnimations[book];
            }
            else if (prefab == 2)
            {
                prefab3Animator.runtimeAnimatorController = CharacterDatabase.CharacterAnimations[book];
            }
            else
                Debug.Log("Trying to update a prefab that isn't one of the 3");
        }
        
        //public void UpdatePrefabHat(int prefab, int hat)
        //{
        //    if (prefab == -1)
        //    {
        //        previewHatSprite.sprite = CharacterDatabase.HatSprites[hat];
        //    }
        //    else if (prefab == 0)
        //    {
        //        prefab1HatSprite.sprite = CharacterDatabase.HatSprites[hat];
        //    }
        //    else if (prefab == 1)
        //    {
        //        prefab2HatSprite.sprite = CharacterDatabase.HatSprites[hat];
        //    }
        //    else if (prefab == 2)
        //    {
        //        prefab3HatSprite.sprite = CharacterDatabase.HatSprites[hat];
        //    }
        //    else
        //        Debug.Log("Trying to update a prefab that isn't one of the 3");
        //}
    }
}

