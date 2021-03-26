using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
    public class Restart
    {
        public static bool restartPressed = false;
    }

	public class RestartAutoPlay : MonoBehaviour 
	{
        public GameManager_Master gameManagerMaster;

        public GameObject UIPlayerPreview;

        private void Start()
        {
            if (Restart.restartPressed)
            {
                gameManagerMaster.GetComponent<GameManager_PlayPressed>().PlayPressed();
                UIPlayerPreview.SetActive(false);
                Restart.restartPressed = false;
            }
        }

        public void RestartPress()
        {
            Restart.restartPressed = true;
        }
    }
}

