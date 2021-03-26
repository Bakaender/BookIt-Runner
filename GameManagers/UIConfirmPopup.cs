using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class UIConfirmPopup : MonoBehaviour 
	{
        public GameObject PopupCanvas;

        private Text messageText;
        private Text buttonText;

        private GameObject spawnedPopup;

        private void Start()
        {
            messageText = PopupCanvas.GetComponentInChildren<Text>();
            buttonText = PopupCanvas.GetComponentInChildren<Button>().GetComponentInChildren<Text>();
        }

        public void ShowPopup(string messageText, string buttonText)
        {
            this.messageText.text = messageText;
            this.buttonText.text = buttonText;
            spawnedPopup = Instantiate(PopupCanvas);

            spawnedPopup.GetComponentInChildren<Button>().onClick.AddListener(DestroyOnConfirm);
        }

        void DestroyOnConfirm()
        {
            Destroy(spawnedPopup);
        }
    }
}

