using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;

namespace BookIt
{
	public class AchievmentAndLeaderboardHandler : MonoBehaviour 
	{
        static UIConfirmPopup UIPopup;
        static string notConnectedMessage;

        private void Start()
        {
            notConnectedMessage = "Not connected to Play Center";
#if UNITY_ANDROID
            notConnectedMessage = "Not connected to Google Play Games";
#endif

            UIPopup = FindObjectOfType<UIConfirmPopup>();
        }

        public static void ShowLeaderboard()
        {
            if (Social.localUser.authenticated) Social.ShowLeaderboardUI();
            else
            {
                MainReferences.onlineServicesConnect.Authenticate(ShowLBAfterAuthenticate);
            }
        }

        static void ShowLBAfterAuthenticate(bool success)
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowLeaderboardUI();
            }
            else { UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Leaderboards", notConnectedMessage); }
        }

        public static void ShowAchievements()
        {
            if (Social.localUser.authenticated) Social.ShowAchievementsUI();
            else
            {
                MainReferences.onlineServicesConnect.Authenticate(ShowAchAfterAuthenticate);
            }
        }

        static void ShowAchAfterAuthenticate(bool success)
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowAchievementsUI();
            }
            else { UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Achievements", notConnectedMessage); }
        }
   
        public static void ReportLeaderboard(long score, string leaderboard)
        {
            if (Social.localUser.authenticated) Social.ReportScore(score, leaderboard, success => {  });
        }

        public static void ReportAchievement(string achievementID, double progress)
        {
            if (Social.localUser.authenticated) Social.ReportProgress(achievementID, progress, success => 
            {
                PlayerPrefs.SetInt(achievementID, 1);
                PlayerPrefs.Save();
            });
        }
    }
}

