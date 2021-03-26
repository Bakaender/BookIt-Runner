using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class CanReviveCurrentScore : MonoBehaviour 
	{
		void OnEnable()
		{
            gameObject.GetComponent<Text>().text = " Score: " + GameManager_Master.score.ToString();
		}
	}
}

