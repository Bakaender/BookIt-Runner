using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class ParallaxOriginReset : MonoBehaviour 
	{
        private GameManager_Master gameManagerMaster;

        public float parallaxSpeed;

        void OnEnable()
		{
			SetInitialReferences();
            gameManagerMaster.ResetToOriginEvent += ResetOrigin;
		}

		void OnDisable()
		{
            gameManagerMaster.ResetToOriginEvent -= ResetOrigin;
        }

		void SetInitialReferences()
		{
            gameManagerMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_Master>();
            if (gameManagerMaster == null)
            {
                Debug.Log("gameManagerMaster null in ParallaxOriginReset.");
            }
        }

        void ResetOrigin()
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - NewWorldGen.originResetDistance * parallaxSpeed, gameObject.transform.position.y, 0);
        }
    }
}

