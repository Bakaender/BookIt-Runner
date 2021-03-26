using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class GameManager_GameOverHandler : MonoBehaviour 
	{
        private GameManager_Master gameManagerMaster;

        public GameObject GamePlayCanvas;
        public GameObject GameOverNoReviveCanvas;
        public GameObject GameOverCanReviveCanvas;

        void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.GameOverEvent += ToggleCanvas;
            
            //Put here since it already has gameplaycanvas and want to minimize stuff have to slot in inspector.
            gameManagerMaster.ToggleGamePlayUIEvent += ToggleGamePlayUI;
        }

        void OnDisable()
        {
            gameManagerMaster.GameOverEvent -= ToggleCanvas;
            gameManagerMaster.ToggleGamePlayUIEvent -= ToggleGamePlayUI;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void ToggleCanvas()
        {
            //HARDCODED pages revive cost.
            if (GameManager_Master.canRevive || (GameManager_Master.canRevivePages && NewSaveGame.Instance.pages >= 10))
            {
                if (GameOverCanReviveCanvas != null)
                {
                    GameOverCanReviveCanvas.SetActive(!GameOverCanReviveCanvas.activeSelf);
                }
                else
                    Debug.Log("GameOver Canvas Not Set in Inspector. GameManager_Master");
            }
            else
            {
                if (GameOverNoReviveCanvas != null)
                {
                    GameOverNoReviveCanvas.SetActive(!GameOverNoReviveCanvas.activeSelf);
                }
                else
                    Debug.Log("GameOver Canvas Not Set in Inspector. GameManager_Master");
            }
        }

        void ToggleGamePlayUI()
        {
            if (GamePlayCanvas != null)
            {
                GamePlayCanvas.SetActive(!GamePlayCanvas.activeSelf);
            }
            else
                Debug.Log("GamePlay Canvas Not Set in Inspector. GameManager_Master");
        }
    }
}

