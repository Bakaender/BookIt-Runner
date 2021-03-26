using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class CanReviveHighestScore : MonoBehaviour 
	{
		void OnEnable()
		{
            gameObject.GetComponent<Text>().text = " High Score: " + NewSaveGame.Instance.achievementSave[(int)Achievements.HighScore, 0].ToString();
		}
	}
}

