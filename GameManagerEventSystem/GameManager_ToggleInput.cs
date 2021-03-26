using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class GameManager_ToggleInput : MonoBehaviour 
	{
        private GameManager_Master gameManagerMaster;

        public GameObject ClickDetection;

        void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.InputToggleEvent += ToggleInputDetection;
        }

        void OnDisable()
        {
            gameManagerMaster.InputToggleEvent -= ToggleInputDetection;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void ToggleInputDetection()
        {
            if (ClickDetection != null)
            {
                ClickDetection.SetActive(!ClickDetection.activeSelf);
            }
            else
                Debug.Log("Click Detection object missing on gameManager_Master.");
        }
    }
}

