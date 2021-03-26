using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class ResolutionToggle : MonoBehaviour 
	{
        public Sprite lowBackground;
        public Sprite highBackground;

        private void OnEnable()
        {
            if (NewSaveGame.Instance.resolution == 1)
            {
                GetComponent<Image>().sprite = lowBackground;
            }
            else
                GetComponent<Image>().sprite = highBackground;
        }

        public void ToggleResolution()
        {
            if (NewSaveGame.Instance.resolution == 0)
            {
                NewSaveGame.Instance.resolution = 1;
                NewSaveGame.Instance.Save();
                Screen.SetResolution(SetResolution.pixelWidth / 2, SetResolution.pixelHeight / 2, true);
                GetComponent<Image>().sprite = lowBackground;
            }
            else
            {
                NewSaveGame.Instance.resolution = 0;
                NewSaveGame.Instance.Save();
                Screen.SetResolution(SetResolution.pixelWidth, SetResolution.pixelHeight, true);
                GetComponent<Image>().sprite = highBackground;
            }
        }
    }
}

