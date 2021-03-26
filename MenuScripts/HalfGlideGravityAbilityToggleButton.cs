using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class HalfGlideGravityAbilityToggleButton : MonoBehaviour 
	{
        public GameObject unlockButton;
        public Sprite onImage;
        public Sprite offImage;

        private void OnEnable()
        {
            if (NewSaveGame.Instance.halfGlideGravityUnlocked == 1)
            {
                unlockButton.SetActive(false);

                if (NewSaveGame.Instance.halfGlideGravityActive == 1)
                {
                    GetComponent<Image>().sprite = onImage;
                }
                else
                {
                    GetComponent<Image>().sprite = offImage;
                }
            }
        }

        public void ToggleAbility()
        {
            if (NewSaveGame.Instance.halfGlideGravityActive == 0)
            {
                NewSaveGame.Instance.halfGlideGravityActive = 1;
                NewSaveGame.Instance.Save();
                GameManager_Master.halfGlideGravity = true;
                GetComponent<Image>().sprite = onImage;
            }
            else
            {
                NewSaveGame.Instance.halfGlideGravityActive = 0;
                NewSaveGame.Instance.Save();
                GameManager_Master.halfGlideGravity = false;
                GetComponent<Image>().sprite = offImage;
            }
        }
    }
}