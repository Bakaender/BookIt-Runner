using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
    public class DisableButtonForXTime : MonoBehaviour
    {
        public float disableTime;
        public Button buttonToDisable;

        public void DisableButton()
        {
            buttonToDisable.interactable = false;

            StartCoroutine(ReEnableButtonAfterDelay());
        }

        private IEnumerator ReEnableButtonAfterDelay()
        {
            yield return new WaitForSeconds(disableTime);

            buttonToDisable.interactable = true;
        }
	}
}