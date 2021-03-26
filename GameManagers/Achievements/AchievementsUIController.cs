using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

namespace BookIt
{
	public class AchievementsUIController : MonoBehaviour 
	{
        public Color32 backgroundColor;
        public Color32 barBackgroundColor;
        public Color32 barFillColor;
        public Color32 buttonColor;

        static Color32 staticBackgroundColor;
        static Color32 staticBarBackgroundColor;
        static Color32 staticBarFillColor;
        static Color32 staticButtonColor;

        public Achievements Achievement;

        public Slider achievementBarSlider;
        public Text achievementProgressText;
        public Text rewardText;
        public Button claimButton;

        private bool maxClaimed;

        private void Start()
        {
            Image[] setColors = GetComponentsInChildren<Image>();

            if (setColors.Length == 4)
            {
                //Background color
                setColors[0].color = staticBackgroundColor;
                //Bar background color
                setColors[1].color = staticBarBackgroundColor;
                //Bar fill color
                setColors[2].color = staticBarFillColor;
                //Button color
                setColors[3].color = staticButtonColor;
            }
        }

        private void OnEnable()
        {
            if (Achievement == Achievements.HighScore)
            {
                staticBackgroundColor = backgroundColor;
                staticBarBackgroundColor = barBackgroundColor;
                staticBarFillColor = barFillColor;
                staticButtonColor = buttonColor;
            }

            if (NewSaveGame.Instance.achievementSave[(int)Achievement, 1] == AchievementsMaster.CombinedAchievements[(int)Achievement].GetLength(0))
            {
                maxClaimed = true;
            }
            else
            { maxClaimed = false; }
                
            UpdateAchievement();
        }

        private void UpdateAchievement()
        {
            claimButton.interactable = false;
            claimButton.GetComponentInChildren<Text>().text = "...";

            if (!maxClaimed)
            {
                achievementBarSlider.value = (float)NewSaveGame.Instance.achievementSave[(int)Achievement, 0] /
                    AchievementsMaster.CombinedAchievements[(int)Achievement][NewSaveGame.Instance.achievementSave[(int)Achievement, 1], 0];

                achievementProgressText.text = NewSaveGame.Instance.achievementSave[(int)Achievement, 0].ToString() + "/" +
                    AchievementsMaster.CombinedAchievements[(int)Achievement][NewSaveGame.Instance.achievementSave[(int)Achievement, 1], 0].ToString();

                rewardText.text = "Reward: " + AchievementsMaster.CombinedAchievements[(int)Achievement]
                                  [NewSaveGame.Instance.achievementSave[(int)Achievement, 1], 1].ToString() + " Pages";

                if (NewSaveGame.Instance.achievementSave[(int)Achievement, 0] >= AchievementsMaster.CombinedAchievements[(int)Achievement][NewSaveGame.Instance.achievementSave[(int)Achievement, 1], 0])
                {
                    claimButton.GetComponentInChildren<Text>().text = "Claim";
                    claimButton.interactable = true;
                }
            }
            else
            {
                achievementBarSlider.value = (float)NewSaveGame.Instance.achievementSave[(int)Achievement, 0] /
                    AchievementsMaster.CombinedAchievements[(int)Achievement][NewSaveGame.Instance.achievementSave[(int)Achievement, 1] - 1, 0];

                achievementProgressText.text = NewSaveGame.Instance.achievementSave[(int)Achievement, 0].ToString() + "/" +
                    AchievementsMaster.CombinedAchievements[(int)Achievement][NewSaveGame.Instance.achievementSave[(int)Achievement, 1] - 1, 0].ToString();

                rewardText.text = "Reward: " + AchievementsMaster.CombinedAchievements[(int)Achievement]
                                  [NewSaveGame.Instance.achievementSave[(int)Achievement, 1] - 1, 1].ToString() + " Pages";

                claimButton.GetComponentInChildren<Text>().text = "Max";
                claimButton.interactable = false;
            } 
        }

        public void ClaimRewardClick()
        {
            NewSaveGame.Instance.pages += AchievementsMaster.CombinedAchievements[(int)Achievement][NewSaveGame.Instance.achievementSave[(int)Achievement, 1], 1];

            NewSaveGame.Instance.achievementSave[(int)Achievement, 1] += 1;
            if (NewSaveGame.Instance.achievementSave[(int)Achievement, 1] == AchievementsMaster.CombinedAchievements[(int)Achievement].GetLength(0))
            {
                maxClaimed = true;
            }

            NewSaveGame.Instance.Save();

            UpdateAchievement();
        }
    }
}