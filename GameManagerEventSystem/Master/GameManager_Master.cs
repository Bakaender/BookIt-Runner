using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
    public enum GameState
    {
        Menu,
        Tutorial,
        Playing,
        Paused,
        GameOver
    }

	public class GameManager_Master : MonoBehaviour 
	{
        //1.1
        public static float fadeDuration;
        public static bool fireImmune = false;
        public static bool halfGlideGravity = false;
        //1.0
        public static GameState gameState;
        public static int score = 0;
        public static bool playPressed = false;
        public static bool isPaused;
        public static bool canRevive;
        public static bool canRevivePages;

        
        #region Events
        public delegate void GameManagerEventHandler();
        public event GameManagerEventHandler ResetToOriginEvent;
        public event GameManagerEventHandler PauseToggleEvent;
        public event GameManagerEventHandler InputToggleEvent;
        public event GameManagerEventHandler ToggleCountdownUIEvent;
        public event GameManagerEventHandler GameOverEvent;

        //Gameover handler subscribes to this.
        public event GameManagerEventHandler ToggleGamePlayUIEvent;
        public event GameManagerEventHandler UpdateBookSkinsSelectedEvent;
        public event GameManagerEventHandler ResetMovingPlatformsEvent;

        //Background cycle event
        //1.1
        public event GameManagerEventHandler CycleBackgroundEvent;

        public delegate void GameManagerEventHandlerInt(int number);
        
        public event GameManagerEventHandlerInt WriteHighScoreEvent;


        //1.1
        private void Start()
        {
            if (NewSaveGame.Instance.fireImmuneUnlocked == 1 && NewSaveGame.Instance.fireImmuneActive ==1)
            {
                GameManager_Master.fireImmune = true;
            }

            if (NewSaveGame.Instance.halfGlideGravityUnlocked == 1 && NewSaveGame.Instance.halfGlideGravityActive == 1)
            {
                GameManager_Master.halfGlideGravity = true;
            }
        }


        public void CallEventPauseToggle()
        {
            if (PauseToggleEvent != null)
            {
                PauseToggleEvent();
            }
        }

        public void CallEventResetToOrigin()
        {
            if (ResetToOriginEvent != null)
            {
                ResetToOriginEvent();
            }
        }

        public void CallEventInputToggle()
        {
            if (InputToggleEvent != null)
            {
                InputToggleEvent();
            }
        }

        public void CallEventToggleCountdownUI()
        {
            if (ToggleCountdownUIEvent != null)
            {
                ToggleCountdownUIEvent();
            }
        }

        public void CallEventToggleGamePlayUI()
        {
            if (ToggleGamePlayUIEvent != null)
            {
                ToggleGamePlayUIEvent();
            }
        }

        public void CallEnvetUpdateBookSkinsSelected()
        {
            if (UpdateBookSkinsSelectedEvent != null)
            {
                UpdateBookSkinsSelectedEvent();
            }
        }

        public void CallEventGameOver()
        {
            if (GameOverEvent != null)
            {
                GameOverEvent();
            }
        }

        public void CallEventResetMovingPlatforms()
        {
            if (ResetMovingPlatformsEvent != null)
            {
                ResetMovingPlatformsEvent();
            }
        }

        //1.1
        public void CallEventCycleBackground()
        {
            if (CycleBackgroundEvent != null)
            {
                CycleBackgroundEvent();
            }
        }

        public void CallEventWriteHighScore(int score)
        {
            if (WriteHighScoreEvent != null)
            {
                WriteHighScoreEvent(score);
            }
        }

        #endregion
    }
}

