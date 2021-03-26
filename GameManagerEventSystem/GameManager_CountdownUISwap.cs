using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class GameManager_CountdownUISwap : MonoBehaviour 
	{
        private GameManager_Master gameManagerMaster;

        public GameObject object1;
        public GameObject object2;

        void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.ToggleCountdownUIEvent += ToggleObjects;
        }

        void OnDisable()
        {
            gameManagerMaster.ToggleCountdownUIEvent -= ToggleObjects;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void ToggleObjects()
        {
            if (object1 != null)
            {
                object1.SetActive(!object1.activeSelf);
            }
            else
                Debug.Log("No object1 on GameManager_Toggle2Objects");

            if (object2 != null)
            {
                object2.SetActive(!object2.activeSelf);
            }
            else
                Debug.Log("No object2 on GameManager_Toggle2Objects");
        }
    }
}

