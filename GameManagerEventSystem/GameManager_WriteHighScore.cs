using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class GameManager_WriteHighScore : MonoBehaviour 
	{
        private GameManager_Master gameManagerMaster;

        void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.WriteHighScoreEvent += SaveHighScore;
        }

        void OnDisable()
        {
            gameManagerMaster.WriteHighScoreEvent -= SaveHighScore;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }
        
        void SaveHighScore(int score)
        {
            NewSaveGame.Instance.NewHighScore(score);
        }
    }
}

