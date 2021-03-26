using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class FireImmuneAbilityToggleButton : MonoBehaviour 
	{
        public GameObject unlockButton;
        public Sprite onImage;
        public Sprite offImage;

        private void OnEnable()
        {
            if (NewSaveGame.Instance.fireImmuneUnlocked == 1)
            {
                unlockButton.SetActive(false);

                if (NewSaveGame.Instance.fireImmuneActive == 1)
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
            if (NewSaveGame.Instance.fireImmuneActive == 0)
            {
                NewSaveGame.Instance.fireImmuneActive = 1;
                NewSaveGame.Instance.Save();
                GameManager_Master.fireImmune = true;
                GetComponent<Image>().sprite = onImage;
            }
            else
            {
                NewSaveGame.Instance.fireImmuneActive = 0;
                NewSaveGame.Instance.Save();
                GameManager_Master.fireImmune = false;
                GetComponent<Image>().sprite = offImage;
            }
        }
    }
}