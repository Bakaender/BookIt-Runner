using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class HalfGlideGravityAbilityUnlockButton : MonoBehaviour 
	{
        public static bool confirmPurchase = false;
        public static bool cancelPurchase = false;

        public string Name = "ability";
        public int UnlockCost = 1000;
        public GameObject ConfirmationPanel;
        public GameObject InsufficientPagesPanel;

        private void OnDisable()
        {
            InsufficientPagesPanel.SetActive(false);
        }

        public void UnlockClicked()
        {
            if (NewSaveGame.Instance.pages >= UnlockCost)
            {
                ConfirmationPanel.GetComponentInChildren<Text>().text = "Spend " + UnlockCost + " pages to unlock " + Name + "?";
                ConfirmationPanel.SetActive(true);

                StartCoroutine(WaitForUnlockConfirm());
            }
            else
            {
                InsufficientPagesPanel.GetComponentInChildren<Text>().text = "Insufficient pages to unlock " + Name +
                                                                             ". You need " + UnlockCost + " Pages.";
                InsufficientPagesPanel.SetActive(true);

                StartCoroutine(HideInsufficientPagesPanel());
            }
        }

        private IEnumerator HideInsufficientPagesPanel()
        {
            yield return new WaitForSeconds(3f);

            InsufficientPagesPanel.SetActive(false);
        }

        private IEnumerator WaitForUnlockConfirm()
        {
            while (!confirmPurchase && !cancelPurchase)
            {
                yield return null;
            }

            yield return new WaitForFixedUpdate();
            FinishUnlock();
        }

        void FinishUnlock()
        {
            if (confirmPurchase)
            {
                NewSaveGame.Instance.pages -= UnlockCost;
                NewSaveGame.Instance.halfGlideGravityUnlocked = 1;
                NewSaveGame.Instance.halfGlideGravityActive = 1;
                NewSaveGame.Instance.Save();
                GameManager_Master.halfGlideGravity = true;
                ConfirmationPanel.SetActive(false);
                confirmPurchase = false;
                gameObject.SetActive(false);
            }
            else if (cancelPurchase)
            {
                ConfirmationPanel.SetActive(false);
                cancelPurchase = false;
            }
        }
    }
}