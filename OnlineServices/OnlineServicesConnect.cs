using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace BookIt
{
	public class OnlineServicesConnect : MonoBehaviour 
	{
        static bool created = false;

        static bool authenticated = false;

        private void Awake()
        {
            if (!created)
            {
                DontDestroyOnLoad(gameObject);
                created = true;
            }
            else
            {
                Destroy(gameObject);
            }

            MainReferences.onlineServicesConnect = this;
        }

        //TODO Verify works
        public void Authenticate(Action<bool> callback)
        {
            if (!authenticated)
            {
#if UNITY_ANDROID
                PlayGamesPlatform.Activate();
                PlayGamesPlatform.Instance.SetDefaultLeaderboardForUI(PlayServices.leaderboard_high_score);
#endif
                //Social.localUser.Authenticate(ProcessAuthentication);
                Social.localUser.Authenticate(success => {
                    if (success)
                    {
                        authenticated = true;
                    }
                    callback(success);
                });
            }
        }

        void ProcessAuthentication(bool success)
        {
            if (success)
            {
                //Debug.Log("Success");

                authenticated = true;


                //#if UNITY_ANDROID
                //                AchievmentAndLeaderboardHandler.ReportLeaderboard(NewSaveGame.Instance.achievementSave[(int)Achievements.HighScore, 0], PlayServices.leaderboard_high_score);
                //                AchievmentAndLeaderboardHandler.ReportLeaderboard(NewSaveGame.Instance.achievementSave[(int)Achievements.PagesCollected1Life, 0], PlayServices.leaderboard_pages_collected_in_1_run);

                //                if (PlayerPrefs.GetInt("playedthegame") == 1 && PlayerPrefs.GetInt(PlayServices.achievement_play_the_game) != 1)
                //                {
                //                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_play_the_game, 100.0f);
                //                }

                //                MainReferences.achievementReporter.ReportScoreAchievementAndroid(NewSaveGame.Instance.achievementSave[(int)Achievements.HighScore, 0]);
                //                MainReferences.achievementReporter.ReportJumpsAchievementAndroid();
                //                MainReferences.achievementReporter.ReportGlidesAchievementAndroid();
                //                MainReferences.achievementReporter.ReportGlidingSuperJumpAchievementAndroid();
                //                MainReferences.achievementReporter.ReportSkinsAchievementAndroid();
                //#elif UNITY_IOS
                //                AchievmentAndLeaderboardHandler.ReportLeaderboard(NewSaveGame.Instance.achievementSave[(int)Achievements.HighScore, 0], IosAchievementIDs.ios_leaderboard_high_score);
                //                AchievmentAndLeaderboardHandler.ReportLeaderboard(NewSaveGame.Instance.achievementSave[(int)Achievements.PagesCollected1Life, 0], IosAchievementIDs.ios_leaderboard_pages_collected_in_1_run);

                //                if (PlayerPrefs.GetInt("playedthegame") == 1 && PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_play_the_game) != 1)
                //                {
                //                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_play_the_game, 100.0f);
                //                }

                //                MainReferences.achievementReporter.ReportScoreAchievementIOS(NewSaveGame.Instance.achievementSave[(int)Achievements.HighScore, 0]);
                //                MainReferences.achievementReporter.ReportJumpsAchievementIos();
                //                MainReferences.achievementReporter.ReportGlidesAchievementIos();
                //                MainReferences.achievementReporter.ReportGlidingSuperJumpAchievementIos();
                //                MainReferences.achievementReporter.ReportSkinsAchievementIos();
                //#endif         
            }
            else
            {
                //Debug.Log("Failed");
            }
        }
    }
}

