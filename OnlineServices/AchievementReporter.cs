using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class AchievementReporter : MonoBehaviour 
	{
        private void Start()
        {
            MainReferences.achievementReporter = this;
        }

        //TODO load all and cache as bools at startup.
        #region Score
        public void ReportScoreAchievementAndroid(int score)
        {
            if (score >= 1000)
            {
                if (PlayerPrefs.GetInt(PlayServices.achievement_high_score_i) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_high_score_i, 100.0f);
                }
            }
            if (score >= 2000)
            {
                if (PlayerPrefs.GetInt(PlayServices.achievement_high_score_ii) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_high_score_ii, 100.0f);
                }
            }
            if (score >= 4000)
            {
                if (PlayerPrefs.GetInt(PlayServices.achievement_high_score_iii) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_high_score_iii, 100.0f);
                }
            }
            if (score >= 6000)
            {
                if (PlayerPrefs.GetInt(PlayServices.achievement_high_score_iv) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_high_score_iv, 100.0f);
                }
            }
            if (score >= 10000)
            {
                if (PlayerPrefs.GetInt(PlayServices.achievement_high_score_v) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_high_score_v, 100.0f);
                }
            }
        }

        public void ReportScoreAchievementIOS(int score)
        {
            if (score >= 1000)
            {
                if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_high_score_i) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_high_score_i, 100.0f);
                }
            }
            if (score >= 2000)
            {
                if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_high_score_ii) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_high_score_i, 100.0f);
                }
            }
            if (score >= 4000)
            {
                if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_high_score_iii) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_high_score_iii, 100.0f);
                }
            }
            if (score >= 6000)
            {
                if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_high_score_iv) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_high_score_iv, 100.0f);
                }
            }
            if (score >= 10000)
            {
                if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_high_score_v) != 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_high_score_v, 100.0f);
                }
            }
        }
        #endregion

        #region Jumps

        public void ReportJumpsAchievementAndroid()
        {
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_jumps_i) != 1)
            { 
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 1000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_jumps_i, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_jumps_ii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 3000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_jumps_ii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_jumps_iii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 5000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_jumps_iii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_jumps_iv) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 7000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_jumps_iv, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_jumps_v) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 12500)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_jumps_v, 100.0f);
                }
            }
        }

        public void ReportJumpsAchievementIos()
        {
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_jumps_i) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 1000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_jumps_i, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_jumps_ii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 3000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_jumps_ii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_jumps_iii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 5000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_jumps_iii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_jumps_iv) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 7000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_jumps_iv, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_jumps_v) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.JumpsTotal, 0] >= 12500)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_jumps_v, 100.0f);
                }
            }
        }

        #endregion

        #region Glides

        public void ReportGlidesAchievementAndroid()
        {
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_glides_i) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 1000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_glides_i, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_glides_ii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 3000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_glides_ii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_glides_iii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 5000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_glides_iii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_glides_iv) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 7000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_glides_iv, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_total_glides_v) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 12500)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_total_glides_v, 100.0f);
                }
            }
        }

        public void ReportGlidesAchievementIos()
        {
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_glides_i) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 1000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_glides_i, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_glides_ii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 3000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_glides_ii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_glides_iii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 5000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_glides_iii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_glides_iv) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 7000)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_glides_iv, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_total_glides_v) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.GlidesTotal, 0] >= 12500)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_total_glides_v, 100.0f);
                }
            }
        }

        #endregion

        #region Gliding super jumps

        public void ReportGlidingSuperJumpAchievementAndroid()
        {
            if (PlayerPrefs.GetInt(PlayServices.achievement_super_jump_while_gliding_i) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_super_jump_while_gliding_i, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_super_jump_while_gliding_ii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 5)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_super_jump_while_gliding_ii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_super_jump_while_gliding_iii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 25)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_super_jump_while_gliding_iii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_super_jump_while_gliding_iv) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 50)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_super_jump_while_gliding_iv, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_super_jump_while_gliding_v) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 100)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_super_jump_while_gliding_v, 100.0f);
                }
            }
        }

        public void ReportGlidingSuperJumpAchievementIos()
        {
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_super_jump_while_gliding_i) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 1)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_super_jump_while_gliding_i, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_super_jump_while_gliding_ii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 5)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_super_jump_while_gliding_ii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_super_jump_while_gliding_iii) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 25)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_super_jump_while_gliding_iii, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_super_jump_while_gliding_iv) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 50)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_super_jump_while_gliding_iv, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_super_jump_while_gliding_v) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] >= 100)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_super_jump_while_gliding_v, 100.0f);
                }
            }
        }

        #endregion

        #region Skins

        public void ReportSkinsAchievementAndroid()
        {
            if (PlayerPrefs.GetInt(PlayServices.achievement_unlock_1_skin) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.UnlockedSkins, 0] >= 2)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_unlock_1_skin, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(PlayServices.achievement_unlock_all_skins) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.UnlockedSkins, 0] >= System.Enum.GetNames(typeof(Characters)).Length)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(PlayServices.achievement_unlock_all_skins, 100.0f);
                }
            }
        }

        public void ReportSkinsAchievementIos()
        {
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_unlock_1_skin) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.UnlockedSkins, 0] >= 2)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_unlock_1_skin, 100.0f);
                }
            }
            if (PlayerPrefs.GetInt(IosAchievementIDs.ios_achievement_unlock_all_skins) != 1)
            {
                if (NewSaveGame.Instance.achievementSave[(int)Achievements.UnlockedSkins, 0] >= System.Enum.GetNames(typeof(Characters)).Length)
                {
                    AchievmentAndLeaderboardHandler.ReportAchievement(IosAchievementIDs.ios_achievement_unlock_all_skins, 100.0f);
                }
            }
        }

        #endregion
    }
}

