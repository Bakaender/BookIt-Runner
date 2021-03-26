using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
    public enum Achievements
    {
        HighScore,
        DistanceTotal,
        PagesCollected1Life,
        PagesCollectedTotal,
        GlidesIn1Life,
        GlidesTotal,
        JumpsIn1Life,
        JumpsTotal,
        DeathsTotal,
        MinutesPlayedTotal,
        UnlockedSkins,
        SuperJumpWhileGliding
    }

	public class AchievementsMaster : MonoBehaviour
    {
        public static int[][,] CombinedAchievements = new int[System.Enum.GetNames(typeof(Achievements)).Length][,];

        public Achievements achievement;
        [Header("Value and Rewards arrays MUST be same size")]
        public int[] achievementValues = new int[1];
        public int[] achievementRewards = new int[1];

        
        private void Awake()
        {
            if (achievementValues.Length != achievementRewards.Length)
            {
                Debug.LogError("Value and Rewards arrays are different sizes. " + achievement + " Achievement");
                return;
            }
            AddAchievementToCombined();
        }

        private void AddAchievementToCombined()
        {
            CombinedAchievements[(int)achievement] = new int[achievementValues.Length, 2];

            for (int x = 0; x < achievementValues.Length; x++)
            {
                CombinedAchievements[(int)achievement][x, 0] = achievementValues[x];
                CombinedAchievements[(int)achievement][x, 1] = achievementRewards[x];
            }
        }
    }
}

