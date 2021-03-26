using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.LuisPedroFonseca.ProCamera2D;

namespace BookIt
{
	public class GameManager_PlayPressed : MonoBehaviour 
	{
        public float logoMoveOffScreenTime;
        public GameObject logo;
        public GameObject clickDetection;
        public GameObject aIPlatformPickup;
        public GameObject gamePlayCanvas;
        public GameObject mainMenuCanvas;
        public RectTransform mainMenuButtonsPanel;

        public void PlayPressed()
        {
            if (PlayerPrefs.GetInt("playedthegame") != 1)
            {
                PlayerPrefs.SetInt("playedthegame", 1);
                PlayerPrefs.Save();
            }

#if UNITY_ANDROID
            if (PlayerPrefs.GetInt(PlayServices.achievement_play_the_game) != 1)
            {
                AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_play_the_game, 100.0f);
            }
#elif UNITY_IOS
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_play_the_game) != 1)
            {
                AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_play_the_game, 100.0f);
            }
#endif

            //In order to wait till lands on next platform I transition gameState in NewMovement checkjump based on this bool.
            GameManager_Master.playPressed = true;
            StartCoroutine(MenuPanelSlide());
        }

        private IEnumerator MenuPanelSlide()
        {
            while (mainMenuButtonsPanel.anchoredPosition.x > -mainMenuButtonsPanel.sizeDelta.x / 2)
            {
                mainMenuButtonsPanel.anchoredPosition -= new Vector2(15f, 0);
                yield return null;
            }
            mainMenuCanvas.SetActive(false);
            yield return new WaitForEndOfFrame();
        }

        public IEnumerator CameraOffset(float offset)
        {
            while (ProCamera2D.Instance.OffsetX <= offset)
            {
                //TODO Change this for camera offset reset speed.
                ProCamera2D.Instance.OffsetX = ProCamera2D.Instance.OffsetX += 0.01f;
                yield return null;
            }

            ProCamera2D.Instance.OffsetX = offset;
            yield return new WaitForEndOfFrame();

            if (NewSaveGame.Instance.tutorial == 1)
            {
                clickDetection.SetActive(true);
            }
           
            aIPlatformPickup.SetActive(false);
            if (Time.timeScale == 1f)
            {
                gamePlayCanvas.SetActive(true);
            }
        }

        public IEnumerator MoveLogoOffScreen()
        {
            float logoMoveSpeed = 10.24f / logoMoveOffScreenTime;

            while (logo.transform.position.x > Camera.main.transform.position.x - 10.24f)
            {
                logo.transform.position -= new Vector3(logoMoveSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }

            yield return new WaitForEndOfFrame();
            logo.SetActive(false);
        }
    }
}

