using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class GameManager_TogglePause : MonoBehaviour 
	{
        private GameManager_Master gameManagerMaster;
        //private bool isPaused = false;

        private void Awake()
        {
            if (Time.timeScale == 0)
            {
                GameManager_Master.isPaused = true;
                GameManager_Master.gameState = GameState.Paused;
            }
            else
            {
                GameManager_Master.isPaused = false;
            }

        }

        void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.PauseToggleEvent += TogglePause;
        }

        void OnDisable()
        {
            gameManagerMaster.PauseToggleEvent -= TogglePause;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void TogglePause()
        {
            if (GameManager_Master.isPaused)
            {
                Time.timeScale = 1;
                GameManager_Master.isPaused = false;
                GameManager_Master.gameState = GameState.Playing;
            }
            else
            {
                Time.timeScale = 0;
                GameManager_Master.isPaused = true;
                GameManager_Master.gameState = GameState.Paused;
            }
        }
    }
}

