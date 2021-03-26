using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BookIt
{
    public class ButtonControllerMaster : MonoBehaviour
    {
        [Tooltip("All the canvas that get opened with buttons. To hide them all when clicking new button")]
        public GameObject[] CanvasToHide = new GameObject[1];

        private UISoundManager uiSoundManager;

        private void Start()
        {
            uiSoundManager = FindObjectOfType<UISoundManager>();
        }

        public void HideAllCanvasBeforeShowingNew()
        {
            for (int i = 0; i < CanvasToHide.Length; i++)
            {
                CanvasToHide[i].SetActive(false);
            }
        }

        public void LoadScene(string sceneName)
        {
            if (sceneName != "")
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("No sceneName, enter scene to load in inspector on button OnClicked");
            }
        }

        public void ResetHighScore()
        {
            NewSaveGame.Instance.NewHighScore(0);
        }

        public void ConfirmUnlock()
        {
            BookSkinSelect.confirmPurchase = true;
        }

        public void CancelUnlock()
        {
            BookSkinSelect.cancelPurchase = true;
        }

        public void MenuButton()
        {
            GameManager_Master.gameState = GameState.Menu;
            GameManager_Master.playPressed = false;
        }

        public void Save()
        {
            NewSaveGame.Instance.Save();
        }

        public void ResetTutorial()
        {
            NewSaveGame.Instance.tutorial = 0;
            NewSaveGame.Instance.Save();
        }

        public void ShowLeaderboards()
        {
            AchievmentAndLeaderboardHandler.ShowLeaderboard();
        }

        public void ShowAchievements()
        {
            AchievmentAndLeaderboardHandler.ShowAchievements();
        }

        //public void Authenticate()
        //{
        //    MainReferences.onlineServicesConnect.Authenticate();
        //}

        public void Add1000Pages()
        {
            NewSaveGame.Instance.pages += 1000;
        }

        public void UIButtonPressSound()
        {
            uiSoundManager.PlayButtonPressSound();
        }

        public void UIBookUnlockSound()
        {
            uiSoundManager.PlayBookUnlockSound();
        }

        public void ResetSaveFile()
        {
            NewSaveGame.NewSave();
        }

        public void ConfirmFireImmuneAbilityUnlock()
        {
            FireImmuneAbilityUnlockButton.confirmPurchase = true;
        }

        public void CancelFireImmuneAbilityUnlock()
        {
            FireImmuneAbilityUnlockButton.cancelPurchase = true;
        }

        public void ConfirmHalfGlideGravityAbilityUnlock()
        {
            HalfGlideGravityAbilityUnlockButton.confirmPurchase = true;
        }

        public void CancelHalfGlideGravityAbilityUnlock()
        {
            HalfGlideGravityAbilityUnlockButton.cancelPurchase = true;
        }
    }
}

