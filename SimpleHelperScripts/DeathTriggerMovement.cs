using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class DeathTriggerMovement : MonoBehaviour 
	{
        private GameObject player;

		void OnEnable()
		{
			SetInitialReferences();
		}
	
		void Update() 
		{
            gameObject.transform.position = new Vector2(player.transform.position.x, gameObject.transform.position.y);
		}

		void SetInitialReferences()
		{
            player = GameObject.FindGameObjectWithTag("Player");
		}
	}
}

